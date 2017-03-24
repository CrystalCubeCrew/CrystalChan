using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimation : IPlayerAnimator
{
    public void playAnimation(Animator crystalChan)
    {
        crystalChan.Play("Idle", 0);
    }

}
