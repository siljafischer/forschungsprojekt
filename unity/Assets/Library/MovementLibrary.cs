using System.Collections;
using UnityEngine;
using Assets.Library;

namespace Assets.Library
{
    // collection of different movements (ONLY movements) --> calls CreatureMover: holds all implementations
    public static class MovementLibrary
    {
        // walk
        public static IEnumerator Walk(CreatureMover mover, float speed = 0.1f, float duration = 2f)
        {
            // timer
            float elapsed = 0f;

            // moves forward (Z)
            Vector2 moveAxis = new Vector2(0f, 1f);
            // gets direction of camera
            Vector3 camForward = Camera.main.transform.forward;
            // only moves horizontal
            camForward.y = 0f;
            camForward.Normalize();

            // move while timer lasts
            while (elapsed < duration)
            {
                // destination: mover.transform.position + camForward
                // first false: run
                // second false: jump
                mover.SetInput(moveAxis, mover.transform.position + camForward, false, false);
                elapsed += Time.deltaTime;
                yield return null;
            }
            // reset for remain standing
            //mover.SetInput(Vector2.zero, Vector3.zero, false, false);
        }

        // jump --> NOT READY
        public static IEnumerator Jump(CreatureMover mover, float speed = 0.1f, float duration = 1f)
        {
            yield return null;
        }


        // run
        public static IEnumerator Run(CreatureMover mover, float speed = 0.5f, float duration = 2f)
        {
            float elapsed = 0f;

            Vector2 moveAxis = new Vector2(0f, 1f);
            Vector3 camForward = Camera.main.transform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            while (elapsed < duration)
            {
                // first true: run
                mover.SetInput(moveAxis, mover.transform.position + camForward, true, false);
                elapsed += Time.deltaTime;
                yield return null;
            }
            mover.SetInput(Vector2.zero, Vector3.zero, false, false);
        }

        // run away --> NOT READY
        public static IEnumerator RunAway(CreatureMover mover)
        {
            // combine walking and running
            yield return MovementLibrary.Walk(mover, speed: 0.1f, duration: 2f);
            yield return MovementLibrary.Run(mover, speed: 0.5f, duration: 2f);
            mover.SetInput(Vector2.zero, Vector3.zero, false, false);

        }
    }
}
