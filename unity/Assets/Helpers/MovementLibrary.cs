using System.Collections;
using UnityEngine;
using Controller; // Für CreatureMover

namespace Assets.Library
{
    public static class MovementLibrary
    {
        // Bewegung durch direktes Verschieben (Lerp)
        /*public static IEnumerator MoveByTransform(GameObject animal, Vector3 delta, float duration)
        {
            Vector3 start = animal.transform.position;
            Vector3 end = start + delta;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                animal.transform.position = Vector3.Lerp(start, end, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            animal.transform.position = end;
        }*/

        // Bewegung über CreatureMover-Komponente
        public static IEnumerator MoveWithCreatureMover(CreatureMovement mover, float speed = 0.1f, float duration = 2f)
        {
            float elapsed = 0f;

            // Testweise: Bewegung nach vorne
            Vector2 moveAxis = new Vector2(0f, 1f); // nach vorne (Z), statt seitlich (X)

            Vector3 camForward = Camera.main.transform.forward;
            camForward.y = 0f; // Wir wollen nicht vertikal, sondern nur auf der horizontalen Ebene bewegen
            camForward.Normalize();

            while (elapsed < duration)
            {
                // Testweise SetInput mit "moveAxis" und Zielposition in Blickrichtung der Kamera
                mover.SetInput(moveAxis, mover.transform.position + camForward, false, false);
                elapsed += Time.deltaTime;
                yield return null;
            }

            mover.SetInput(Vector2.zero, Vector3.zero, false, false); // Eingabe zurücksetzen
        }


    }
}
