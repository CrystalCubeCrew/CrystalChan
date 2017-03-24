using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAnimation : IPlayerAnimator {

    public void playAnimation(Animator crystalChan)
    {
        crystalChan.Play("Wave", 0);

    }
}
