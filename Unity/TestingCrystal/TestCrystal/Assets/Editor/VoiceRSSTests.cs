using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class VoiceRSSTests {

    PlayMusic tts;

    [SetUp]
    public void SetUp()
    {
        tts = new PlayMusic();

    }

    [Test]
    public void ifValidStringIsPassedCrystalAnimationIsNoShrugged()
    {
        tts.words = "hello";
       // tts.playSong();

        Assert.False(tts.words.Equals(""));
    }
}
