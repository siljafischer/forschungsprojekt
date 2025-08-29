// CHANGE --> AKTUELL NOCH AN DEN ELCH ANGEPASST
using System.Collections;
using UnityEngine;
using Assets.Library;
using GLTFast.Schema;

namespace Assets.Library
{
    // collection of different movements e.g. Normal Behaviour or RunAway --> CreatureMover: holds all implementations eg Walk, Eat, run --> not necessary (weil gutes animiertes Modell)
    public static class BearMovementLibrary
    {
        // timer
        public static IEnumerator Timer(float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        // MOVEMENTS
        public static IEnumerator MoveUnseen(Animator animator, Transform Transform, float duration, float speed)
        {
            float elapsed = 0f;

            // stand up, run forward and stop
            elapsed = 0f;
            animator.SetBool("isStandingUp", true);
            yield return Timer(duration);
            while (elapsed < duration - 1)
            {
                Transform.position += Transform.forward * speed * Time.deltaTime;
                elapsed += Time.deltaTime;
                yield return null;
            }
            animator.SetBool("isStandingUp", false);
            animator.SetBool("isStopping", true);

            // turn left
            elapsed = 0f;
            animator.SetBool("isTurningLeft", true);
            yield return Timer(duration - 2);
            Transform.rotation = Quaternion.Euler(0, -75f, 0);
            //animator.SetBool("isTurningLeft", false);
            // animator.SetBool("isRunningLeft", true);
            // yield return Timer(duration - 2);
            speed = 1.25f;
            while (elapsed < 6)
            {
                Transform.position += Transform.forward * speed * Time.deltaTime;
                elapsed += Time.deltaTime;
                yield return null;
            }
            animator.SetBool("isStopping", false);
            animator.SetBool("isTurningLeft", false);
            animator.SetBool("isStopping", true);
            animator.SetBool("isSittingDown", true);
            yield return Timer(duration - 2.5f);
            animator.SetBool("isSleeping", true);
        }
    }
}
