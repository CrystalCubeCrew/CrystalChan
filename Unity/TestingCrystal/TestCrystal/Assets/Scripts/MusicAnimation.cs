using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAnimation : IPlayerAnimator
{
    public void playAnimation(Animator crystalChan)
    {
        crystalChan.SetBool("isDancing", true);
    }

    public void stopAnimation(Animator crystalChan)
    {
        crystalChan.SetBool("isDancing", false);
    }
}
