// defines movements from ithappy/Animals_FREE (Asset Store)
using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Library
{
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class CreatureMoverElk : MonoBehaviour
    {
        // walk --> controller
        public void Walk(Animator animator)
        {
            animator.SetBool("isWalking", true);
        }

        public void StopWalk(Animator animator)
        {
            animator.SetBool("isWalking", false);
        }
    }
}