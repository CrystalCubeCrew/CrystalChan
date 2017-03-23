using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsFeedAnimation : IPlayerAnimator {

    public void playAnimation(Animator crystalChan)
    {
        crystalChan.SetBool("isNews", true);
    }

    public void stopAnimation(Animator crystalChan)
    {
        crystalChan.SetBool("isNews", false);
    }
}
