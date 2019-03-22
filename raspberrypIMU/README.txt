BerryIMU Source Files

Raspberry Pi Code:
- both scripts run continuously once started, quit with Ctrl-C
- when main.py exits, both scripts will go back to the setup state, waiting for the laptop IP address over MQTT
wheel.py 
	- this runs on the steering wheel pi
    - it sends angle measurements to the laptop via UDP
	- the connection is setup using an initial MQTT server to get the laptop IP address
    - measures steering angle and forward/backward tilt
	- angle values are computed by combining gyro and accelerometer data in a kalman filter
	- uses BerryIMU library for sensor readings
    - values are mapped to a range from -1 to 1 in order to be used in Unity
pedal.py 
	- this is run on the gas pedal
    - sends pedal angle measurements to laptop via UDP
    - measures forward/backward angle of pedal as it is pressed

Laptop Code
main.py 
	- this program is run from the laptop and serves as a UDP communication hub
    - it receives data from the two sensors and forwards it to Unity
    - when prompted by UDP messages from Unity, speech or image functions will be called
    - this serves as the connection between our Python and C# code
    - when the unity game quits, this will exit as well and send a message back to the sensors
    - NOTE: this must be started before the unity program, so that the UDP sockets are properly setup