# import the necessary packages
from imutils import paths
import numpy as np
import imutils
import cv2
 
def find_marker(image):
	# convert the image to grayscale, blur it, and detect edges
	#hsv = cv2.cvtColor(image, cv2.COLOR_BGR2HSV) 
	
	#rgb = cv2.cvtColor(mask, cv2.COLOR_HSV2RGB)	
	#gray = cv2.cvtColor(rgb, cv2.COLOR_RGB2GRAY)	
	gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
	gray = cv2.GaussianBlur(gray, (9, 9), 0) 

	edged = cv2.Canny(gray, 35, 125)
 
	# find the contours in the edged image and keep the largest one;
	# we'll assume that this is our piece of paper in the image
	cnts = cv2.findContours(edged.copy(), cv2.RETR_LIST, cv2.CHAIN_APPROX_SIMPLE)
	cnts = cnts[0] if imutils.is_cv2() else cnts[1]
	c = max(cnts, key = cv2.contourArea)
 
	# compute the bounding box of the of the paper region and return it
	return cv2.minAreaRect(c)

def distance_to_camera(knownWidth, focalLength, perWidth):
	# compute and return the distance from the maker to the camera
	return (knownWidth * focalLength) / perWidth

# initialize the known distance from the camera to the object, which
# in this case is 24 inches
KNOWN_DISTANCE = 24.0
 
# initialize the known object width, which in this case, the piece of
# paper is 12 inches wide
KNOWN_WIDTH = 11.0
 
# load the furst image that contains an object that is KNOWN TO BE 2 feet
# from our camera, then find the paper marker in the image, and initialize
# the focal length
#image = cv2.imread("images/2ft.png")
cap = cv2.VideoCapture(0)
ret, frame = cap.read()
marker = find_marker(frame)
focalLength = (marker[1][0] * KNOWN_DISTANCE) / KNOWN_WIDTH
while(True):
	ret, frame = cap.read()
	marker = find_marker(frame)
	inches = distance_to_camera(KNOWN_WIDTH, focalLength, marker[1][0])
	print inches/12
	box = cv2.cv.BoxPoints(marker) if imutils.is_cv2() else cv2.boxPoints(marker)
        box = np.int0(box)
    	cv2.drawContours(frame, [box], -1, (0, 255, 0), 2)
	cv2.putText(frame, "%.2fft" % (inches / 12),
		(frame.shape[1] - 200, frame.shape[0] - 20), cv2.FONT_HERSHEY_SIMPLEX,
		2.0, (0, 255, 0), 3)
	#gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
	#hsv = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV) 
	#lower_red = np.array([10,10,50]) 
	#upper_red = np.array([255,255,130]) 
	#mask = cv2.inRange(hsv, lower_red, upper_red) 
    # Display the resulting frame
	#res = cv2.bitwise_and(frame,frame, mask= mask) 
	cv2.imshow('frame',frame) 
	#cv2.imshow('mask',mask) 
	#cv2.imshow('res',res) 
	
	boundaries = [
	([50, 10, 4], [220, 120, 80])]

	for (lower, upper) in boundaries:
		# create NumPy arrays from the boundaries
		lower = np.array(lower, dtype = "uint8")
		upper = np.array(upper, dtype = "uint8")
 
		# find the colors within the specified boundaries and apply
		# the mask
		mask = cv2.inRange(frame, lower, upper)
		output = cv2.bitwise_and(frame, frame, mask = mask)
 
		# show the images
		#cv2.imshow("output", output)

	if cv2.waitKey(1) & 0xFF == ord('q'):
		break

cap.release()
cv2.destroyAllWindows()
