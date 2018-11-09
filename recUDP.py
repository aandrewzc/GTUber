import socket
import paho.mqtt.client as mqtt

UDP_IP = socket.gethostname()
UDP_PORT = 5005

# MQTT connection to ensure proper UDP_IP address is used
def on_connect(client, userdata, flags, rc):
    if rc == 0:
        print("Broker connection successful")
    else:
        print("Error connecting to broker")

# broker to connect to
broker = "broker.hivemq.com"
topic = "ece180d/gtuber/unity_ip" 

print("Creating new instance")
client = mqtt.Client()
client.on_connect = on_connect

client.connect(broker)  # connect to broker
client.loop_start()  # start the loop

print("Subscribing to topic")
client.subscribe(topic)

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.bind((UDP_IP, UDP_PORT))

while True:
	print("Publishing IP address")
	client.publish(topic, UDP_IP)
	# data, addr = sock.recvfrom(1024) # buffer size is 1024 bytes
	# print("received message: %s" % data)
