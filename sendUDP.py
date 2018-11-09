import time
import socket
import paho.mqtt.client as mqtt

ip_flag = False
UDP_IP = "0"
# broker to connect to
broker = "broker.hivemq.com"
topic = "ece180d/gtuber/unity_ip" 

# callback function for connection
def on_connect(client, userdata, flags, rc):
    if rc == 0:
        print("Broker connection successful")
    else:
        print("Error connecting to broker")

# callback for messages
def on_message(client, userdata, message):
    global ip_flag
    global UDP_IP
    print("%s" % str(message.payload))
    UDP_IP = str(message.payload)
    ip_flag = True
    
print("Creating new instance")
client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message

print("Connecting to broker: %s" % broker)
client.connect(broker)
client.loop_start()

print("Subscribing to topic: %s" % topic)
client.subscribe(topic)

while not ip_flag:
    print("Waiting for laptop address")
    time.sleep(1)

client.publish(topic, "ACK")
client.loop_stop()
client.disconnect()

# UDP_IP = "192.168.1.113"
UDP_PORT = 5005

addr = (UDP_IP, UDP_PORT)
message = "Hello, world!"

print("UDP target IP: %s" % UDP_IP)
print("UDP target port: %s" % UDP_PORT)
print("message: %s" % message)

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
while True:
    sock.sendto(message, addr)
    print("sent message: %s" % message)
    time.sleep(1)
