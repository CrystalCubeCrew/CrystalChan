using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToDoAnimation : IPlayerAnimator {

    public void playAnimation(Animator crystalChan)
    {
        crystalChan.Play("ToDoAnimation", 0);

    }
}
