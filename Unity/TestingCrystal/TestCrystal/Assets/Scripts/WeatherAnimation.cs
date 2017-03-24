using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherAnimation :IPlayerAnimator {

    public void playAnimation(Animator crystalChan)
    {
        crystalChan.Play("Weather", 0);

    }
}
