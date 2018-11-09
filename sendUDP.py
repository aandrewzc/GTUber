import socket
import paho.mqtt.client as mqtt

global ip_received

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
    print("%s" % (str(message.payload.decode("utf-8"))))
    UDP_IP = str(message.payload.decode("utf-8"))


print("Creating new instance")
client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message

print("Connecting to broker: %s" % broker)
client.connect(broker)
client.loop_start()

print("Subscribing to topic: %s" % topic)
client.subscribe(topic)

client.loop_forever()


# UDP_IP = "192.168.1.113"
UDP_PORT = 5005

addr = (UDP_IP, UDP_PORT)
message = "Hello, world!"

print("UDP target IP: %s" % UDP_IP)
print("UDP target port: %s" % UDP_PORT)
print("message: %s" % message)

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.sendto(message, addr)
