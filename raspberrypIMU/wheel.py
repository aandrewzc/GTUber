#!/usr/bin/python

'''
wheel.py

To be run from the steering wheel raspberry pi.

Sends steering directions to a laptop program via a UDP connection
Laptop ip address is shared via an MQTT server.

Turning depends on Y angle of IMU.

'''

import sys, signal
import time
import socket
import paho.mqtt.client as mqtt 

import threading
import math
import IMU
import datetime
import os

DEBUG = 0
DATA = 0
USE_MQTT = 1


class ExitThread(threading.Thread):
    def run(self):
        print("exit thread started")
        global UDP_PORT
        rec_sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        rec_addr = (get_ip(), UDP_PORT)
        print("%s" % rec_addr[0])

        rec_sock.bind(rec_addr)

        msg, dummy = rec_sock.recvfrom(1024)
        if msg == "Ctrl-C":
            print("^C")
            sock.sendto("ACK", addr)
            sock.close()
            os._exit(1)


# cleaner output when exiting
def handle_ctrl_c(signal, frame):
    global sock, raw, mapped
    try:
        sock.close()
    except:
        pass

    if DATA:
        raw.close()
        mapped.close()

    os._exit(130)
    # sys.exit(130) # 130 is standard exit code for ctrl-c

#This will capture exit when using Ctrl-C
signal.signal(signal.SIGINT, handle_ctrl_c)


# MQTT callback functions
def on_connect(client, userdata, flags, rc):
    if rc == 0:
        print("Broker connection successful")
    else:
        print("Error connecting to broker")

def on_message(client, userdata, message):
    global ip_flag
    global UDP_IP
    if message.payload[0:3] != b"ACK":
        print("%s" % str(message.payload))
        UDP_IP = message.payload
        ip_flag = True


# get the correct ip address (not 127.0.0.1)
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


# If the IMU is upside down (Skull logo facing up), change this value to 1
IMU_UPSIDE_DOWN = 1

RAD_TO_DEG = 57.29578
M_PI = 3.14159265358979323846
G_GAIN = 0.070  # [deg/s/LSB]  If you change the dps for gyro, you need to update this value accordingly
AA =  0.40      # Complementary filter constant


#Kalman filter variables
Q_angle = 0.02
Q_gyro = 0.0015
R_angle = 0.005
y_bias = 0.0
x_bias = 0.0
XP_00 = 0.0
XP_01 = 0.0
XP_10 = 0.0
XP_11 = 0.0
YP_00 = 0.0
YP_01 = 0.0
YP_10 = 0.0
YP_11 = 0.0
KFangleX = 0.0
KFangleY = 0.0


def kalmanFilterY ( accAngle, gyroRate, DT):
    y=0.0
    S=0.0

    global KFangleY
    global Q_angle
    global Q_gyro
    global y_bias
    global YP_00
    global YP_01
    global YP_10
    global YP_11

    KFangleY = KFangleY + DT * (gyroRate - y_bias)

    YP_00 = YP_00 + ( - DT * (YP_10 + YP_01) + Q_angle * DT )
    YP_01 = YP_01 + ( - DT * YP_11 )
    YP_10 = YP_10 + ( - DT * YP_11 )
    YP_11 = YP_11 + ( + Q_gyro * DT )

    y = accAngle - KFangleY
    S = YP_00 + R_angle
    K_0 = YP_00 / S
    K_1 = YP_10 / S
    
    KFangleY = KFangleY + ( K_0 * y )
    y_bias = y_bias + ( K_1 * y )
    
    YP_00 = YP_00 - ( K_0 * YP_00 )
    YP_01 = YP_01 - ( K_0 * YP_01 )
    YP_10 = YP_10 - ( K_1 * YP_00 )
    YP_11 = YP_11 - ( K_1 * YP_01 )
    
    return KFangleY

def kalmanFilterX ( accAngle, gyroRate, DT):
    x=0.0
    S=0.0

    global KFangleX
    global Q_angle
    global Q_gyro
    global x_bias
    global XP_00
    global XP_01
    global XP_10
    global XP_11

    KFangleX = KFangleX + DT * (gyroRate - x_bias)

    XP_00 = XP_00 + ( - DT * (XP_10 + XP_01) + Q_angle * DT )
    XP_01 = XP_01 + ( - DT * XP_11 )
    XP_10 = XP_10 + ( - DT * XP_11 )
    XP_11 = XP_11 + ( + Q_gyro * DT )

    x = accAngle - KFangleX
    S = XP_00 + R_angle
    K_0 = XP_00 / S
    K_1 = XP_10 / S
    
    KFangleX = KFangleX + ( K_0 * x )
    x_bias = x_bias + ( K_1 * x )
    
    XP_00 = XP_00 - ( K_0 * XP_00 )
    XP_01 = XP_01 - ( K_0 * XP_01 )
    XP_10 = XP_10 - ( K_1 * XP_00 )
    XP_11 = XP_11 - ( K_1 * XP_01 )
    
    return KFangleX


# IMU.detectIMU()     #Detect if BerryIMUv1 or BerryIMUv2 is connected.
IMU.initIMU()       #Initialise the accelerometer, gyroscope and compass

gyroXangle = 0.0
gyroYangle = 0.0
gyroZangle = 0.0
CFangleX = 0.0
CFangleY = 0.0
kalmanX = 0.0
kalmanY = 0.0


# flag for connection setup
ip_flag = False

# UDP IP and port number variables
# UDP_IP = "192.168.1.113"
UDP_IP = "131.179.4.54"
UDP_PORT = 11000

if USE_MQTT:
    # Setup MQTT connection
    broker = "broker.hivemq.com"
    topic = "ece180d/gtuber/unity_ip"

    print("Creating new instance")
    client = mqtt.Client()
    client.on_connect = on_connect
    client.on_message = on_message

    print("Connecting to %s" % broker)

    # try until connection successful
    while True:
        try:
            client.connect(broker) 
        except:
            pass
        else:
            break
            
    client.loop_start()

    print("Subscribing to %s" % topic)
    client.subscribe(topic)

    # poll until IP address received           
    print("Waiting for laptop address")
    while not ip_flag:
       time.sleep(1)

    # send ACK and close server connection
    ack = "ACK wheel" + get_ip()
    client.publish(topic, ack)
    client.loop_stop()
    client.disconnect()

# setup UDP connection
addr = (UDP_IP, UDP_PORT)

print("UDP target IP: %s" % UDP_IP)
print("UDP target port: %s" % UDP_PORT)

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.setblocking(0)

exit_thread = ExitThread()
exit_thread.start()

if DATA:
    raw = open("wheel_raw.txt", "w")
    mapped = open("wheel_mapped.txt", "w")

a = datetime.datetime.now()
while True:
    #Read the accelerometer,gyroscope and magnetometer values
    ACCx = IMU.readACCx()
    ACCy = IMU.readACCy()
    ACCz = IMU.readACCz()
    GYRx = IMU.readGYRx()
    GYRy = IMU.readGYRy()
    GYRz = IMU.readGYRz()
    
    ##Calculate loop Period(LP). How long between Gyro Reads
    b = datetime.datetime.now() - a
    a = datetime.datetime.now()
    LP = b.microseconds/(1000000*1.0)

    if DEBUG:
    	print("Loop Time %5.2f " % ( LP ))

    #Convert Gyro raw to degrees per second
    rate_gyr_x =  GYRx * G_GAIN
    rate_gyr_y =  GYRy * G_GAIN
    rate_gyr_z =  GYRz * G_GAIN

    #Calculate the angles from the gyro. 
    gyroXangle+=rate_gyr_x*LP
    gyroYangle+=rate_gyr_y*LP
    gyroZangle+=rate_gyr_z*LP

    #Convert Accelerometer values to degrees
    if not IMU_UPSIDE_DOWN:
        # If the IMU is up the correct way (Skull logo facing down), use these calculations
        AccXangle =  (math.atan2(ACCy,ACCz)*RAD_TO_DEG)
        AccYangle =  (math.atan2(ACCz,ACCx)+M_PI)*RAD_TO_DEG
    else:
        #Us these four lines when the IMU is upside down. Skull logo is facing up
        AccXangle =  (math.atan2(-ACCy,-ACCz)*RAD_TO_DEG)
        AccYangle =  (math.atan2(-ACCz,-ACCx)+M_PI)*RAD_TO_DEG

    #Change the rotation value of the accelerometer to -/+ 180 and
    #move the Y axis '0' point to up.  This makes it easier to read.
    if AccYangle > 90:
        AccYangle -= 270.0
    else:
        AccYangle += 90.0

    #Complementary filter used to combine the accelerometer and gyro values.
    CFangleX=AA*(CFangleX+rate_gyr_x*LP) +(1 - AA) * AccXangle
    CFangleY=AA*(CFangleY+rate_gyr_y*LP) +(1 - AA) * AccYangle

    #Kalman filter used to combine the accelerometer and gyro values.
    kalmanY = kalmanFilterY(AccYangle, rate_gyr_y,LP)
    kalmanX = kalmanFilterX(AccXangle, rate_gyr_x,LP)

    # -90 = right turn = +1
    # +90 = left turn = -1

    value = -kalmanY/90.0
    if value > 1:
        value = 1
    elif value < -1:
        value = -1

    tilt = kalmanX/90.0
    sock.sendto(b"w:%.2f,%.2f" % (value, tilt), addr)

    if DEBUG:
    	print("AngleY: %d, AngleX: %d, value: %.2f, tilt: %.2f" % (kalmanY, kalmanX, value, tilt))
    	#slow program down a bit, makes the output more readable
    	time.sleep(0.03)

    if DATA:
        raw.write("%.2f,%.2f\n" % (kalmanY, kalmanX))
        mapped.write("%.2f,%.2f\n" % (value, tilt))
