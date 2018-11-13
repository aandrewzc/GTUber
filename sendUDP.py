'''
sendUDP.py

Script to send IMU data to the laptop, run from the Raspberry Pi

Waits to receive laptop IP address via MQTT server.
Once receieved, communication can continue via UDP.
'''

import time
import socket
import paho.mqtt.client as mqtt 

# MQTT callback functions
def on_connect(client, userdata, flags, rc):
    if rc == 0:
        print("Broker connection successful")
    else:
        print("Error connecting to broker")

def on_message(client, userdata, message):
    global ip_flag
    global UDP_IP
    print("%s" % str(message.payload))
    UDP_IP = str(message.payload)
    ip_flag = True

# flag for connection setup
ip_flag = False

# UDP IP and port number variables
UDP_IP = "0"
UDP_PORT = 5005

# Setup MQTT connection
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

# poll until IP address received
while not ip_flag:
    print("Waiting for laptop address")
    time.sleep(1)

# send ACK and close server connection
ack = "ACK" + socket.gethostbyname(socket.gethostname())
client.publish(topic, ack)
client.loop_stop()
client.disconnect()

# setup UDP connection
addr = (UDP_IP, UDP_PORT)
message = "Hello, world!"

print("UDP target IP: %s" % UDP_IP)
print("UDP target port: %s" % UDP_PORT)
print("message: %s" % message)

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
count = 0
while True:
    count += 1
    sock.sendto(message, addr)
    sock.sendto(str(count), addr)
    print("sent message: %s %d" % (message, count))
    time.sleep(1)
