BerryIMU Source Files

Raspberry Pi Code:
wheel.py - this is run on the steering wheel 
    - it sends angle measurements to the laptop via UDP
    - measures steering angle and forward/backward tilt
    - the values are mapped to a range from -1 to 1 in order to be used in Unity
pedal.py - this is run on the gas pedal
    - sends pedal angle measurements to laptop via UDP
    - measures forward/backward angle of pedal as it is pressed

Laptop Code
main.py - this program is run from the laptop and serves as a UDP communication hub
    - it receives data from the two sensors and forwards it to Unity
    - when prompted by UDP messages from Unity, speech or image functions will be called
    - this serves as the connection between our Python and C# code