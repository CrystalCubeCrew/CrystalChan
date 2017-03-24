using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMathAnimation : IPlayerAnimator
{
    public void playAnimation(Animator crystalChan)
    {
        crystalChan.Play("mathAnimation", 0);

    }
}
