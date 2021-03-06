# CrystalChan

Crystal-Chan is the Avatar that the user will see on the front end. It will be designed and animated using Blender and any audio sound effects for the avatar to use will be created and/or edited using Audacity. Crystal-Chan will grab response information from the CrystalCube itself. The response information will be strings of text that tell the Crystal-Chan component what animation to play. Crystal-Chan will play the animation that it was told to play as the CrystalCube itself played the audio response through its speakers. Methodical commands are processed with Crystal-Chan through the Unity3D environment. These commands include playing a particular animation. Methods for playing a particular animation will be coded using C#. The CrystalCube will send a request to the method for playing an animation. This would be a method call with a string passed as a parameter stating which animation to play. With this information  the method will determine which animation should be played and call another method to play that animation. Crystal-Chan’s animation will then be displayed on the LCD screen on the CrystalCube and the transparent film will show the animation in a holographic way.


# How do i run crystal chan?
You will also need to have the CrystalCloud component ReadMe setup to have a cloud side for Crystal. PocketCrystal ReadMe must also be set up so facial recognition login can be used. 
Once you have those set up you can start your setup for Crystal-Chan.

You will need:
You will need the following package, Android Ultimate Package for speech, if you would like to edit the code for crystal chan:https://www.assetstore.unity3d.com/en/#!/content/45168
You might also need: https://www.assetstore.unity3d.com/en/#!/content/31498 for api.ai connection
Unity version 5.5.1f1
NUnit Package installed to edit test code
Blender 2.75a
Next you can download the source code from our git repository at the link: https://github.com/CrystalCubeCrew/CrystalChan


# How you boot crystal on android os?

Make sure that you have Android version 23.
Ensure that you have Google TTS installed on your device and that it has the first female voice on the list as the voice of Google TTS. To download a female voice you must install the voice pack (female voice 1st on the list). Lastly, in your voice settings ensure that you have TTS (text to speech) being US - United States. Also, Crystal is apart of the CrystalCube and users would buy the cube with everything pre installed and ready to go.

To run Crystal on the device, navigate to the builds folder once you download the repository: CrystalChan/Unity/TestingCrystal/TestCrystal/Builds/
And select and download the .apk on your android device.


# What can Crystal do?

Crystal can...
Answer weather, to-do list, basic math, news, and login related questions.

The user must first create an account via PocketCrystal, the mobile applicaton for CrystalCube. Then, the user can face the CrystalCube camera and say "Hey Crystal, [pause till she sounds a beep and is listening] login" and Crystal will greet the user.
The following functionalities will now be about to be implmented via voice command:

Weather:
What to say?
Hey Crystal, What is the weather like in [state or city]

To-do list:
What to say?
Hey Crystal, What is on my to do list?

Basic Math:
What to say?
Hey Crystal, What is [-number-] [-operation-] [-number-]?

operation can be: multiplied by, divided by, minus, times, plus.

News:
What to say?
Hey Crystal, What is news in [sports, technology, the world]




# Credits

The Crystal - Chan model itself:
CC-BY: Attribution — You must give appropriate credit, provide a link to the license, and indicate if changes were made. You may do so in any reasonable manner, but not in any way that suggests the licensor endorses you or your use.

Author: mirza
License link: https://creativecommons.org/licenses/by/3.0/
Changes: Changes were made to the texture and model changes were also made.
