'''
sendUDP.py

Script to send IMU data to the laptop, run from the Raspberry Pi

Waits to receive laptop IP address via MQTT server.
Once receieved, communication can continue via UDP.
'''

import time
import socket

# UDP IP and port number variables
UDP_IP = "127.0.0.1"
UDP_PORT = 11000

# setup UDP connection
addr = (UDP_IP, UDP_PORT)
message = "Hello, world!"

print("UDP target IP: %s" % UDP_IP)
print("UDP target port: %s" % UDP_PORT)
print("message: %s" % message)

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
#sock.setblocking(0)
#time.sleep(3)

count = 0
while True:
    count += 1
    sock.sendto(message + str(count), addr)
    print("sent message: %s %d" % (message, count))
    time.sleep(1)
