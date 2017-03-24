using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAnimation : IPlayerAnimator
{
    public void playAnimation(Animator crystalChan)
    {
        crystalChan.Play("Dancing", 0);
    }
}
