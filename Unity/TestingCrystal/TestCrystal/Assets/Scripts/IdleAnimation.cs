using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimation : IPlayerAnimator
{
    public void playAnimation(Animator crystalChan)
    {
        crystalChan.SetBool("isIdle", true);
    }

    public void stopAnimation(Animator crystalChan)
    {
        crystalChan.SetBool("isIdle", false);
    }
}
