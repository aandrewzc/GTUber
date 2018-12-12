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
	
def filter_to_color (frame):
	
	gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
	hsv = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV) 
	lower_red = np.array([0,30,0]) 
	upper_red = np.array([120,255,50])
	mask = cv2.inRange(hsv, lower_red, upper_red) 
		# Display the resulting frame
	res = cv2.bitwise_and(frame,frame, mask= mask) 

		
	boundaries = [
		([0, 30, 0], [120, 255, 50])]

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
	return output
	
	
def	drunkTest():

	# initialize the known distance from the camera to the object, which
	# in this case is 24 inches
	KNOWN_DISTANCE = 24.0
	 
	# initialize the known object width, which in this case, the piece of
	# paper is 12 inches wide
	KNOWN_WIDTH = 9.0
	 
	# load the furst image that contains an object that is KNOWN TO BE 2 feet
	# from our camera, then find the paper marker in the image, and initialize
	# the focal length
	#image = cv2.imread("images/2ft.png")

	desiredDistance = 84;
	returnThreshold  = 24;

	cap = cv2.VideoCapture(0)

	while(True):
		try:
			ret, frame = cap.read()
			output = filter_to_color(frame)
			marker = find_marker(output)
			print(marker[1][0])
			break
		except:
			pass

	focalLength = (marker[1][0] * KNOWN_DISTANCE) / KNOWN_WIDTH

	distanceNotReached = True;
	returnNotReached = True;
	while(distanceNotReached or returnNotReached):
	
		while(True):
			try:
				ret, frame = cap.read()
				output = filter_to_color(frame)
				marker = find_marker(output)
				break
			except:
				pass
		
		
		
		inches = distance_to_camera(KNOWN_WIDTH, focalLength, marker[1][0])
		
		if(distanceNotReached == False):
			if inches < returnThreshold:
				returnNotReached = False
		if inches > desiredDistance:
			distanceNotReached = False
		
		
		
		
		#box = cv2.cv.BoxPoints(marker) if imutils.is_cv2() else cv2.boxPoints(marker)
		
		#if imutils.is_cv2()
		#	box = cv2.cv.BoxPoints(marker)
		#else
		#	box = cv2.boxPoints(marker)	

		box = cv2.boxPoints(marker)
		box = np.int0(box)
		cv2.drawContours(output, [box], -1, (0, 255, 0), 2)
		cv2.putText(output, "%.2fft" % (inches / 12), (output.shape[1] - 200, output.shape[0] - 20), cv2.FONT_HERSHEY_SIMPLEX, 2.0, (0, 255, 0), 3)
		cv2.imshow('output',output) 

		if cv2.waitKey(1) & 0xFF == ord('q'):
			break

	cap.release()
	cv2.destroyAllWindows()
	return
	
drunkTest()
