import speech_recognition as sr

CREDENTIALS = r"""{
    "type": "service_account",
    "project_id": "speech-recognition-225304",
    "private_key_id": "c4265c256aaadf4be36148970d470c7cbc45ab8c",
    "private_key": "-----BEGIN PRIVATE KEY-----\nMIIEuwIBADANBgkqhkiG9w0BAQEFAASCBKUwggShAgEAAoIBAQCnoHL41/gSi+yL\nbYIyVKtVp+C4ELYMANEfcCfSbKzzGlUpTKholvXf5atHd6pjCE9Jlhpa0h6ppJ2z\nqRt8sIFCHPprpZiyCR4TIT8kT9a39M606iOYPlyUEQShze6Ntv3uN4pxEkzwEiSR\namctqJ44CvdZ7oRIlFT+BWnW1D9G0P82L8Qjh1NCex4xncrjm3ZS7Z6BTfAlKfIY\nyGAzMOZOVXgMvWug1bsgkpg4J+u34XUoTtD2bTPpoRcYk+N0CmlbDRzd2INOuhJO\nfP+eAc3chqUHzZ18jYUUxG/cog6LIUXiiGLct6ULjxlazLeO1Hz7H8DkFdwWQz47\ncQZgNtl5AgMBAAECggEAArs5sF4VhdjXlICG7PLF/W3wOpOUIc8wGSTq0zIP4eCx\nl0wPWO/vxvOPNkmIAlfX8eUjHgx7MSy0YKFuaBpYqtb9yhv/SFRfc/SYWqs93o67\nwQUpKhGHEOcTlh7cxpyfF976D6RaklVbWO68ymjLVjKwi9u39lZLab21Fynrc4gM\nwqfjBoIt5yD7cgtACsrKVxCkLzDAzsLPhMvFn0oAlvq7USlpXoaZl1boA0se0le/\nLqAM/mCyrDK4AoXnV8kKQSx3VIYRRTgPnR2ljv8lVIG03bRr6eCPmKPs643cinx2\nRrGljXktT2EFewzu4jInKHnaQJlmsJMaKtSUt9TisQKBgQDdDvrOEki+G6z9ars/\nxXshrNcgA6b09jocJ1SxPCYvAL49CHkqdR+P5/bJK78gM2XHkg/kQ2IRIQEiC+4b\nQ+TjiAHIFjqxL0OzdtbP9mgS0R5pAvHkiMBqQuxnEG/m+uRuofFEQ7ZvlOXjtiKz\nY7KvFKs4hknz9UPNNqH268XYkQKBgQDCH2FjKzxi6h8bxCvKL6L7HWrJ7P27vegR\nQBpfz13uS5Dd2QfdlB7m8Wjujgpf1A55zmgCcSHWi4Ulzl5vWnTsWNHa+F2nJJEr\nlofJ6mA1Fvcc9yegybwmfPmNSE7Hf7IYVMalgYvS5EmIF7HLFQnOd/fCXL2p3YET\nmczr0POmaQKBgD5S09k3hHS8vsfvyH0RQ0baQQiQnwifRwDbAeC6MZW84uW4NtkG\njnB3GLfc8pS0iPR8NKjJaqOjp3se2/Gp8aVImh/8TG3aAn9dePDG6s6rUKSQpPwm\npgEPfzELjFEV2UNruubhJFVPKIVLu3rSF/WOoTtdeQoj0wrPZViFXpFRAoGBAK6n\nNSPYPphwNFTzBsL+YRC1wuKySwejMSx0FEand8dyTYCMR7tgDP2TrekT+tmFHRES\nWBn6V4uEM+Nz7Rsas6ABua3dT8WV7NbetIVCThT1+Jci95B+cL3gK1al+9saVM33\nWFKIX5j6s/2x89DXuN7+qPdCIZK3K+7KrchtUwohAn95qv4v5wy0aGEH3kAlLuXC\n0/iPZD3Zun+teBa57IA3zU9tN939BzK54+OtsLRU7FXR+Yz9exFZuOi/v/WbnAwl\n/2hEqyCMKoNrf5fl8TZw0upPG3C7YVQAnmOTucTe1rrYhJOU4ZQPrI1pUITll4+P\nyiGQg9+G9oxedeupeNVc\n-----END PRIVATE KEY-----\n",
    "client_email": "danatovey@speech-recognition-225304.iam.gserviceaccount.com",
    "client_id": "115329888069318664555",
    "auth_uri": "https://accounts.google.com/o/oauth2/auth",
    "token_uri": "https://oauth2.googleapis.com/token",
    "auth_provider_x509_cert_url": "https://www.googleapis.com/oauth2/v1/certs",
    "client_x509_cert_url": "https://www.googleapis.com/robot/v1/metadata/x509/danatovey%40speech-recognition-225304.iam.gserviceaccount.com"
    }
    """

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
        # recognizer.adjust_for_ambient_noise(source)
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
    TARGET = "Uber for " + name
    PROMPT_LIMIT = 5
    
    # create recognizer and mic instances
    recognizer = sr.Recognizer()
    microphone = sr.Microphone()

    # listen to microphone and make guess
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
    guess_is_correct = guess["transcription"].lower().rstrip() == TARGET.lower()
    return guess_is_correct

if __name__ == "__main__":
    test_correct_guess = test_for_passenger("Joe")
    if test_correct_guess:
        print("Guess was correct")
    else:
        print("Guess was not correct")
