using System.Collections;
using UnityEngine;
using Assets.Library;
using GLTFast.Schema;

namespace Assets.Library
{
    // collection of different movements (ONLY movements) --> calls CreatureMover: holds all implementations
    public static class MovementLibraryElk
    {
        // walk
        public static IEnumerator Walk(Animator animator, Transform elkTransform, float duration, float speed)
        {
            float elapsed = 0f;

            // start animation
            animator.SetBool("isWalking", true);
            while (elapsed < duration)
            {
                // slide forward
                elkTransform.position += elkTransform.forward * speed * Time.deltaTime;
                elapsed += Time.deltaTime;
                yield return null;
            }
            // stop walking
            animator.SetBool("isWalking", false);
        }
    }
}
