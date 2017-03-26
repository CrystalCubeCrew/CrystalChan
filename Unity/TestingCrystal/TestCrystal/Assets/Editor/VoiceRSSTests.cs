using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class VoiceRSSTests {

    VoiceRSSTextToSpeech tts;

    [SetUp]
    public void SetUp()
    {
        tts = new VoiceRSSTextToSpeech();

    }

    [Test]
    public void ifValidStringIsPassedCrystalAnimationIsNoShrugged()
    {
        tts.words = "hello";
        tts.playTextToSpeech();

        Assert.False(tts.words.Equals(""));
    }
}
