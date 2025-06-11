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


        // walk normal
        public static IEnumerator WalkNormal(CreatureMover mover)
        {
            // variables for camera and position
            float elapsed = 0f;
            Vector3 camForward = Camera.main.transform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            // walk backward
            Vector2 moveBackward = new Vector2(0f, 1f);
            while (elapsed < 3f)
            {
                mover.SetInput(moveBackward, mover.transform.position + camForward, false, false);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // stand 1 sec
            mover.SetInput(Vector2.zero, mover.transform.position, false, false);
            yield return new WaitForSeconds(1f);

            // turn and walk right
            elapsed = 0f;
            Vector2 moveRight = new Vector2(1f, 0f);
            while (elapsed < 2f)
            {
                mover.SetInput(moveRight, mover.transform.position + camForward, false, false);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // stand 1 sec
            mover.SetInput(Vector2.zero, mover.transform.position, false, false);
            yield return new WaitForSeconds(1f);

            // turn and walk left
            elapsed = 0f;
            Vector2 moveLeft = new Vector2(-1f, 0f);
            while (elapsed < 2f)
            {
                mover.SetInput(moveLeft, mover.transform.position + camForward, false, false);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // stand 1 sec
            mover.SetInput(Vector2.zero, mover.transform.position, false, false);
            yield return new WaitForSeconds(1f);

            // turn and walk forward
            elapsed = 0f;
            Vector2 moveForward = new Vector2(0f, -1f);
            while (elapsed < 1f)
            {
                mover.SetInput(moveForward, mover.transform.position + camForward, false, false);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // stand 1 sec
            mover.SetInput(Vector2.zero, mover.transform.position, false, false);
            yield return new WaitForSeconds(1f);

            // turn and walk RightBackward
            elapsed = 0f;
            Vector2 moveRightBackward = new Vector2(1f, 1f);
            while (elapsed < 1f)
            {
                mover.SetInput(moveRightBackward, mover.transform.position + camForward, false, false);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // end
            mover.SetInput(Vector2.zero, mover.transform.position, false, false);

        }
        public static IEnumerator Turn90Right(CreatureMover mover)
        {
            float spinDuration = 0.25f;
            float spinSpeed = 360f;
            float spinElapsed = 0f;

            // turn 90 left
            while (spinElapsed < spinDuration)
            {
                float rotationThisFrame = spinSpeed * Time.deltaTime;
                mover.transform.Rotate(0, rotationThisFrame, 0);
                spinElapsed += Time.deltaTime;
                yield return null;
            }
        }

        public static IEnumerator Turn90Left(CreatureMover mover)
        {
            float spinDuration = 0.25f;
            float spinSpeed = 360f;
            float spinElapsed = 0f;

            // turn 90 left
            while (spinElapsed < spinDuration)
            {
                float rotationThisFrame = spinSpeed * Time.deltaTime;
                mover.transform.Rotate(0, -rotationThisFrame, 0);
                spinElapsed += Time.deltaTime;
                yield return null;
            }
        }

        // run away --> NOT READY
        public static IEnumerator RunAway(CreatureMover mover)
        {

            // Stoppen nach der Drehung
            mover.SetInput(Vector2.zero, mover.transform.position, false, false);
            yield return new WaitForSeconds(0.1f);

            // variables for camera and position
            float elapsed = 0f;
            Vector3 camForward = Camera.main.transform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            // turn and walk RightBackward
            elapsed = 0f;
            Vector2 moveRightBackward = new Vector2(1f, 1f);
            while (elapsed < 3f)
            {
                mover.SetInput(moveRightBackward, mover.transform.position + camForward, true, false);
                elapsed += Time.deltaTime;
                yield return null;
            }
            // stand 0.1 sec
            mover.SetInput(Vector2.zero, mover.transform.position, false, false);
            yield return new WaitForSeconds(0.1f);

            // turn and walk left
            elapsed = 0f;
            Vector2 moveLeft = new Vector2(-1f, 0f);
            while (elapsed < 4f)
            {
                mover.SetInput(moveLeft, mover.transform.position + camForward, true, false);
                elapsed += Time.deltaTime;
                yield return null;
            }
            // stand 0.1 sec
            mover.SetInput(Vector2.zero, mover.transform.position, false, false);
            yield return new WaitForSeconds(0.1f);

            // turn and walk RightBackward
            elapsed = 0f;
            while (elapsed < 5f)
            {
                mover.SetInput(moveRightBackward, mover.transform.position + camForward, true, false);
                elapsed += Time.deltaTime;
                yield return null;
            }
            // stand 1 sec
            mover.SetInput(Vector2.zero, mover.transform.position, false, false);


            elapsed = 0f;
            Vector2 moveRight = new Vector2(1f, 0f);
            while (elapsed < 3f)
            {
                mover.SetInput(moveRight, mover.transform.position + camForward, true, false);
                elapsed += Time.deltaTime;
                yield return null;
            }

            //end
            mover.SetInput(Vector2.zero, Vector3.zero, false, false);
        }
    }
}
