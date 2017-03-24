using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsFeedAnimation : IPlayerAnimator {

    public void playAnimation(Animator crystalChan)
    {
        crystalChan.Play("NewsFeedAnimation", 0);

    }
}
