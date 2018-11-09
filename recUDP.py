import socket
import time
import paho.mqtt.client as mqtt

UDP_IP = socket.gethostbyname(socket.gethostname())
UDP_PORT = 5005

connected_flag = False
ack_flag = False

# MQTT connection to ensure proper UDP_IP address is used
def on_connect(client, userdata, flags, rc):
    if rc == 0:
        global connected_flag 
        connected_flag = True
        print("Broker connection successful")
    else:
        print("Error connecting to broker")

def on_message(client, userdata, message):
	global ack_flag
	if str(message.payload) == "ACK":
		print("%s" % str(message.payload))
		ack_flag = True 

# broker to connect to
broker = "broker.hivemq.com"
topic = "ece180d/gtuber/unity_ip" 

print("Creating new instance")
client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message

client.connect(broker)  # connect to broker
client.loop_start()  # start the loop

print("Subscribing to topic")
client.subscribe(topic)

while not connected_flag:
	print("Waiting for connection")
	time.sleep(1)

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

while True:
	data, addr = sock.recvfrom(1024) # buffer size is 1024 bytes
	print("received message: %s" % data)
