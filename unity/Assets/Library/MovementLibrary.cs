using System.Collections;
using UnityEngine;
using Assets.Library;
using GLTFast.Schema;

namespace Assets.Library
{
    // collection of different movements e.g. Normal Behaviour or RunAway --> CreatureMover: holds all implementations eg Walk, Eat, run --> not necessary (weil gutes animiertes Modell)
    public static class MovementLibrary
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
        // lay
        public static IEnumerator Chill(Animator animator, float duration)
        {
            // start animation --> lay down and wait
            animator.SetBool("isLying", true);
            yield return Timer(duration+5);
            // stand up and stop
            animator.SetBool("isLying", false);
        }
        public static IEnumerator MoveUnseen(Animator animator, Transform Transform, float duration, float speed)
        {
            float elapsed = 0f;

            for (int i = 0; i <= 2; i++)
            {
                elapsed = 0f;

                animator.applyRootMotion = false;
                // eating
                animator.SetBool("isEating", true);
                yield return Timer(duration - 1);
                animator.SetBool("isEating", false);

                // turn right
                animator.SetBool("isRight", true);
                yield return Timer(duration);
                animator.SetBool("isRight", false);
                // walk right
                Transform.rotation = Quaternion.Euler(0, 90f, 0);
                animator.SetBool("isWalking", true);
                while (elapsed < duration - 2)
                {
                    Transform.position += Transform.forward * speed * Time.deltaTime;
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                animator.SetBool("isWalking", false);

                // turn left
                animator.SetBool("isLeft", true);
                yield return Timer(duration - 2);
                animator.SetBool("isLeft", false);
                Transform.rotation = Quaternion.Euler(0, 0f, 0);
                animator.SetBool("isWalking", true);
                while (elapsed < duration)
                {
                    Transform.position += Transform.forward * speed * Time.deltaTime;
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                animator.SetBool("isWalking", false);

                // chill
                yield return Chill(animator, duration);
                yield return Timer(duration);
            }

            // eating
            animator.SetBool("isEating", true);
            yield return Timer(duration - 1);
            animator.SetBool("isEating", false);

            // turn and walk forward
            elapsed = 0f;
            animator.SetBool("isRight", true);
            yield return Timer(duration);
            animator.SetBool("isRight", false);
            Transform.rotation = Quaternion.Euler(0, 180f, 0);
            animator.SetBool("isWalking", true);
            while (elapsed < duration)
            {
                Transform.position += Transform.forward * speed * Time.deltaTime;
                elapsed += Time.deltaTime;
                yield return null;
            }
            animator.SetBool("isWalking", false);
            
            // turn and walk right
            elapsed = 0f;
            animator.SetBool("isRight", true);
            yield return Timer(duration);
            animator.SetBool("isRight", false);
            Transform.rotation = Quaternion.Euler(0, 90f, 0);
            animator.SetBool("isWalking", true);
            while (elapsed < duration - 2)
            {
                Transform.position += Transform.forward * speed * Time.deltaTime;
                elapsed += Time.deltaTime;
                yield return null;
            }
            animator.SetBool("isWalking", false);

        }

        public static IEnumerator RunAway(Animator animator, Transform Transform, float duration, float speed)
        {
            float elapsed = 0f;

            animator.applyRootMotion = false;
            // eating
            animator.SetBool("isEating", true);
            yield return Timer(duration - 1);
            animator.SetBool("isEating", false);

            // run forward
            animator.SetBool("isRunning", true);
            while (elapsed < duration-2)
            {
                Transform.position += Transform.forward * speed * Time.deltaTime;
                elapsed += Time.deltaTime;
                yield return null;
            }
            animator.SetBool("isRunning", false);

            // run right
            animator.SetBool("isRunningRight", true);
            yield return Timer(0.3f);
            Transform.rotation = Quaternion.Euler(0, 90f, 0);
            animator.SetBool("isRunningRight", false);

            elapsed = 0f;
            animator.SetBool("isRunning", true);
            while (elapsed < duration - 2)
            {
                Transform.position += Transform.forward * speed * Time.deltaTime;
                elapsed += Time.deltaTime;
                yield return null;
            }
            animator.SetBool("isRunning", false);

            // run left and mysteriously disappear
            animator.SetBool("isRunningLeft", true);
            yield return Timer(0.3f);
            Transform.rotation = Quaternion.Euler(0, 180f, 0);
            animator.SetBool("isRunningLeft", false);
        }
    }
}
