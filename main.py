#!/usr/local/bin/python

'''
main.py

Script to receive IMU data, run on a laptop

Communicates machine's IP address via an MQTT server connection.
Once this is established, communication can continue via UDP.
'''

import sys, signal
import time
import socket
import paho.mqtt.client as mqtt
import threading
import os

import speech_part
import dist_vid_color

USE_MQTT = 1
NUM_PIS = 2

send_data = True
check_passenger = False
drunk_test = False 

passenger_name = ""


class ListenUnity(threading.Thread):
	def run(self):
		global LOCAL_IP, send_data, check_passenger, drunk_test, passenger_name

		print("listen thread started")
		rec_sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
		rec_addr = (LOCAL_IP, 8881)
		print("Listening at %s" % rec_addr[0])
		rec_sock.bind(rec_addr)
		rec_sock.setblocking(0)

		while True:
			try:
				msg, dummy = rec_sock.recvfrom(1024)
				print(msg)
				# if (msg == "pickup"):
				msg_type = msg.split(":")[0]
				if (msg_type == b"pickup"):
					send_data = False
					check_passenger = True
					passenger_name = msg.split(":")[1]

				elif (msg == b"police"):
					send_data = False
					check_passenger = False
					drunk_test = True 
					
				elif (msg == b"quit"):
					kill_sensors();
					os._exit(130)

			except:
				pass

				

def kill_sensors():
	global udp_sock, local_sock, wheel_addr, pedal_addr
	
	# quit wheel program (if connected)
	try:
		udp_sock.sendto(b"Ctrl-C", wheel_addr)
		print("Sent Ctrl-C to wheel at %s" % wheel_addr[0])
	except:
		pass

	# quit pedal program (if connected)
	try:
		udp_sock.sendto(b"Ctrl-C", pedal_addr)
		print("Sent Ctrl-C to pedal at %s" % pedal_addr[0])
	except:
		pass

	# close sockets (if created)
	try:
		udp_sock.close()
	except:
		pass

	try:
		local_sock.close()
	except:
		pass


def handle_ctrl_c(signal, frame):
	kill_sensors()
	os._exit(1)


#This will capture exit when using Ctrl-C
signal.signal(signal.SIGINT, handle_ctrl_c)


# MQTT callback functions
def on_connect(client, userdata, flags, rc):
    if rc == 0:
        global connected_flag 
        connected_flag = True
        print("Broker connection successful")
    else:
        print("Error connecting to broker")

def on_message(client, userdata, message):
	global ack_flag, wheel_addr, pedal_addr, UDP_PORT

	if message.payload[0:3] == b"ACK":
		print("%s" % str(message.payload))	
		if message.payload[4:9] == b"wheel":
			wheel_addr = (message.payload[9:], UDP_PORT)
			print("wheel connected at %s" % wheel_addr[0])
		elif message.payload[4:9] == b"pedal":
			pedal_addr = (message.payload[9:], UDP_PORT)
			print("pedal connected at %s" % pedal_addr[0])
		ack_flag += 1 


# get ip address of the laptop (not 127.0.0.1)
# this catches and error in finding my mac's ip address on my home wifi 
def get_ip():
	try:
		my_ip = socket.gethostbyname(socket.gethostname())
		if my_ip.startswith("127."):
			raise
	except:
		s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
		s.connect(("8.8.8.8", 53))
		my_ip = s.getsockname()[0]
		s.close()
	return my_ip


# flags for connection setup
connected_flag = False
ack_flag = 0

# UDP IP and port number
UDP_IP = get_ip()
UDP_PORT = 11000

LOCAL_IP = "127.0.0.1"
LOCAL_PORT = 11000

# pedal and wheel addresses
wheel_addr = ("", 0)
pedal_addr = ("", 0)


print(UDP_IP)

if USE_MQTT:
	# setup MQTT connection
	broker = "broker.hivemq.com"
	topic = "ece180d/gtuber/unity_ip" 

	print("Creating new instance")
	client = mqtt.Client()
	client.on_connect = on_connect
	client.on_message = on_message

	print("Connecting to %s" % broker)
	client.connect(broker)
	client.loop_start()

	print("Subscribing to %s" % topic)
	client.subscribe(topic)

	# poll until connected to server
	while not connected_flag:
		print("Waiting for connection")
		time.sleep(1)

	# publish IP address until an ACK is received
	print("Waiting for ACKs")
	while ack_flag < NUM_PIS:
		# print(UDP_IP)
		client.publish(topic, UDP_IP)
		time.sleep(1)

	client.loop_stop()
	client.disconnect()

# setup UDP connection
print("Setting up UDP connection")
udp_addr = (UDP_IP, UDP_PORT)
udp_sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
try:
	udp_sock.bind(udp_addr)
except Exception as e:
	print("Check that Unity receive socket closed properly: " + str(e))
	kill_sensors()
	os._exit(1)

udp_sock.setblocking(0)

local_addr = (LOCAL_IP, LOCAL_PORT)
local_sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
local_sock.setblocking(0)

listen_thread = ListenUnity()
listen_thread.start()

#dist_vid_color.drunkTest(dist_vid_color.calibration())

while True:
	if send_data:
		try:
			data, addr = udp_sock.recvfrom(1024)
			# print("received message: %s from %s" % (data, addr[0]))
			local_sock.sendto(data, local_addr)
		except:
			pass

	elif check_passenger: 
		while check_passenger:
			print("Checking passenger identity")
			try:
				correct_answer = speech_part.test_for_passenger(passenger_name)
			except:
				correct_answer = True

			result = b"pickup:" + passenger_name + b","

			if correct_answer:
				print("Identity verified!")
				local_sock.sendto(result + b"correct", local_addr)
				check_passenger = False
				send_data = True
			else:
				print("Wrong passenger...")
				local_sock.sendto(result + "incorrect", local_addr)

	elif drunk_test:
		print("Drunk test underway")
		time.sleep(10)
		if(dist_vid_color.drunkTest(dist_vid_color.calibration())):
			drunk_test = False
			send_data = True 
			local_sock.sendto(b"police:pass", local_addr)
		else:
			drunk_test = False
			send_data = True
			local_sock.sendto(b"police:fail", local_addr)






