#!/usr/local/bin/python

'''
recUDP.py

Script to receive IMU data, run on a laptop

Communicates machine's IP address via an MQTT server connection.
Once this is established, communication can continue via UDP.
'''

import sys, signal
import time
import socket
import paho.mqtt.client as mqtt

USE_MQTT = 1
NUM_PIS = 2
count = 0

def handle_ctrl_c(signal, frame):
	global count, udp_sock, local_sock
	print(count)

	# tell the wheel and pedal to exit
	exit_flag = 0
	while not exit_flag:
		try:
			try:
				udp_sock.sendto("Ctrl-C", wheel_addr)
				print("Sent Ctrl-C to wheel at %s" % wheel_addr[0])
			except:
				pass

			try:
				udp_sock.sendto("Ctrl-C", pedal_addr)
				print("Sent Ctrl-C to pedal at %s" % wheel_addr[0])
			except:
				pass

			exit_flag = 1
			udp_sock.close()

		except NameError:
			print("exiting")
			break

	sys.exit(130) # 130 is standard exit code for ctrl-c

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
	if str(message.payload)[0:3] == "ACK":
		print("%s" % str(message.payload))	
		if str(message.payload)[4:9] == "wheel":
			wheel_addr = (str(message.payload)[9:], UDP_PORT)
			print("wheel connected at %s" % wheel_addr[0])
		elif str(message.payload)[4:9] == "pedal":
			pedal_addr = (str(message.payload)[9:], UDP_PORT)
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
udp_sock.bind(udp_addr)
udp_sock.setblocking(0)

local_addr = ("127.0.0.1", 11000)
local_sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

while True:
	try:
		data, addr = udp_sock.recvfrom(1024)
		print("received message: %s from %s" % (data, addr[0]))
		# local_sock.sendto(data, local_addr)
	except:
		pass
	count += 1


