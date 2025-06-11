using System.Collections;
using UnityEngine;
using Assets.Library;

namespace Assets.Library
{
    // collection of different movements (ONLY movements) --> calls CreatureMover: holds all implementations
    public static class MovementLibraryElk
    {
        // walk
        public static IEnumerator Walk(CreatureMoverElk mover, Animation anim, float duration = 5f)
        {
            // timer
            float elapsed = 0f;

            // move while timer lasts
            while (elapsed < duration)
            {
                mover.Eat(anim);
                elapsed += Time.deltaTime;
            }
            // reset timer and wait for 1 second
            elapsed = 0f;
            yield return new WaitForSeconds(1f);

            // turn left
            mover.WalkLeft(anim);
            // move while timer lasts
            while (elapsed < duration)
            {
                mover.Walk(anim);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}
