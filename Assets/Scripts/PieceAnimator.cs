using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceAnimator : MonoBehaviour
{

    //private bool DEBUG_clips = true;
    //private bool DEBUG_audio = true;

    /* --- Animation ---*/

    public AnimationClip[] animations;

    public Animator animator;

    public void SetAnimation(int level)
    {
        if (gameObject.activeSelf)
        {
            animator.Play(animations[level - 1].name);
        }
    }
}
