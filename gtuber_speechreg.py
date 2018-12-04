import speech_recognition as sr

def recognize_speech_from_mic(recognizer, microphone):
    """Transcribe speech from recorded from `microphone`.

    Returns a dictionary with three keys:
    "success": a boolean indicating whether or not the API request was
               successful
    "error":   `None` if no error occured, otherwise a string containing
               an error message if the API could not be reached or
               speech was unrecognizable
    "transcription": `None` if speech could not be transcribed,
               otherwise a string containing the transcribed text
    """
    # check that recognizer and microphone arguments are appropriate type
    if not isinstance(recognizer, sr.Recognizer):
        raise TypeError("`recognizer` must be `Recognizer` instance")

    if not isinstance(microphone, sr.Microphone):
        raise TypeError("`microphone` must be `Microphone` instance")

    # adjust the recognizer sensitivity to ambient noise and record audio
    # from the microphone
    with microphone as source:
        recognizer.adjust_for_ambient_noise(source)
        audio = recognizer.listen(source)

    # set up the response object
    response = {
        "success": True,
        "error": None,
        "transcription": None
    }

    # try recognizing the speech in the recording
    # if a RequestError or UnknownValueError exception is caught,
    #     update the response object accordingly
    try:
        response["transcription"] = recognizer.recognize_google(audio)
    except sr.RequestError:
        # API was unreachable or unresponsive
        response["success"] = False
        response["error"] = "API unavailable"
    except sr.UnknownValueError:
        # speech was unintelligible
        response["error"] = "Unable to recognize speech"

    return response


def test_for_passenger(name):
    
    # set the target name, prompt limit
    TARGET = "Uber for " + name;
    PROMPT_LIMIT = 5;
    
    # create recognizer and mic instances
    recognizer = sr.Recognizer()
    microphone = sr.Microphone()

    # listen to microphone and make guess
    guess = recognize_speech_from_mic(recognizer, microphone)
    for i in range(PROMPT_LIMIT):
        print("Speak into the microphone.")
        guess = recognize_speech_from_mic(recognizer, microphone)
        if guess["transcription"]:
            break
        if not guess["success"]:
            break
        print("I didn't catch that. What did you say?\n")

    # if there was an error, stop the game
    if guess["error"]:
        print("ERROR: {}".format(guess["error"]))

    # show the user the transcription
    print("You said: {}".format(guess["transcription"]))
    
    # determine if guess is correct and return answer
    guess_is_correct = guess["transcription"].lower() == TARGET.lower()
    return guess_is_correct


if __name__ == "__main__":
    test_correct_guess = test_for_passenger("Joe")
    if test_correct_guess:
        print("Correct! You win!")
    else:
        print("Guess was not correct")

