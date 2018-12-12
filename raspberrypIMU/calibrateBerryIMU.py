#!/usr/bin/python
#   This program is used to calibrate the compass on a BerryIMUv1 or
#   BerryIMUv2.
#
#   Start this program and rotate your BerryIMU in all directions. 
#   You will see the maximum and minimum values change. 
#   After about 30secs or when the values are not changing, press Ctrl-C.
#   The program will printout some text which you then need to add to
#   berryIMU.py or berryIMU-simple.py


import sys,signal,os
import time
import math

import IMU
import datetime


def handle_ctrl_c(signal, frame):
    global count, total
    print("averages: x_drift = %5.2f y_drift = %5.2f z_drift = %5.2f" % (total[0]/count, total[1]/count, total[2]/count))

    sys.exit(130) # 130 is standard exit code for ctrl-c

# IMU.detectIMU()
IMU.initIMU()

#This will capture exit when using Ctrl-C
signal.signal(signal.SIGINT, handle_ctrl_c)
G_GAIN = 0.070  # [deg/s/LSB]  If you change the dps for gyro, you need to update this value accordingly

count = 0
total = [0,0,0]
while True:
    #Read magnetometer values
    GYRx = IMU.readGYRx()
    GYRy = IMU.readGYRy()
    GYRz = IMU.readGYRz()

    rate_gyr_x =  GYRx * G_GAIN
    rate_gyr_y =  GYRy * G_GAIN
    rate_gyr_z =  GYRz * G_GAIN
    
    total[0] += rate_gyr_x
    total[1] += rate_gyr_y
    total[2] += rate_gyr_z
    count += 1

    print("x:%5.2f  y:%5.2f  z:%5.2f" % (rate_gyr_x, rate_gyr_y, rate_gyr_z))

    #slow program down a bit, makes the output more readable
    time.sleep(0.03)


