using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrugAnimation :IPlayerAnimator
{
    public void playAnimation(Animator crystalChan)
    {
        crystalChan.SetBool("isShrugging", true);
    }

    public void stopAnimation(Animator crystalChan)
    {
        crystalChan.SetBool("isShrugging", false);
    }
}
