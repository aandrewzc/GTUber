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

def handle_ctrl_c(signal, frame):
	global count
	print(count)
	sys.exit(130) # 130 is standard exit code for ctrl-c

#This will capture exit when using Ctrl-C
signal.signal(signal.SIGINT, handle_ctrl_c)

# setup UDP connection
print("Setting up UDP connection")
local_addr = ("127.0.0.1", 11000)
local_sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
local_sock.bind(local_addr)
local_sock.setblocking(0)

count = 0
while True:
	try:
		data, addr = local_sock.recvfrom(1024)
		print("received message: %s from %s" % (data, addr[0]))
	except:
		pass
	count += 1


