using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrugAnimation :IPlayerAnimator
{
    public void playAnimation(Animator crystalChan)
    {
        crystalChan.Play("Shrug", 0);

    }
}
