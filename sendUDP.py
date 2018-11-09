import socket

UDP_IP = "192.168.1.113"
UDP_PORT = 5005

addr = (UDP_IP, UDP_PORT)
message = "Hello, world!"

print("UDP target IP: %s" % UDP_IP)
print("UDP target port: %s" % UDP_PORT)
print("message: %s" % message)

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.sendto(message, addr)
