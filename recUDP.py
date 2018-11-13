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

def handle_ctrl_c(signal, frame):
	global count
	print(count)
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
	global ack_flag
	global PI_IP
	if str(message.payload)[0:3] == "ACK":
		print("%s" % str(message.payload))
		PI_IP = str(message.payload)[3:]
		ack_flag = True 

# flags for connection setup
connected_flag = False
ack_flag = False

# UDP IP and port number
UDP_IP = socket.gethostbyname(socket.gethostname())
UDP_PORT = 5005

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
while not ack_flag:
	print(UDP_IP)
	client.publish(topic, UDP_IP)
	time.sleep(1)

client.loop_stop()
client.disconnect()

# setup UDP connection
print("Setting up UDP connection")
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.bind((UDP_IP, UDP_PORT))
sock.setblocking(0)

count = 0
while True:
	try:
		data, addr = sock.recvfrom(1024)
		print("received message: %s from %s" % (data, addr[0]))
		sock.sendto("ACK", addr)
		print("sent ACK")
	except:
		pass
	count += 1


