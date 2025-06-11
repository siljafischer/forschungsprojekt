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
        public void Stand(Animation anim)
        {
            anim.Play("Stand_Breathing_01");
        }
        public void Eat(Animation anim)
        {
            anim.Play("Stand_Eating_01");
        }
        public void Walk(Animation anim)
        {
            anim.Play("Walk");
        }
        public void WalkLeft(Animation anim)
        {
            anim.Play("WalkL");
        }
        public void WalkRight(Animation anim)
        {
            anim.Play("WalkR");
        }
        public void Run(Animation anim)
        {
            anim.Play("Run");
        }
        public void RunLeft(Animation anim)
        {
            anim.Play("RunL");
        }
        public void RunRight(Animation anim)
        {
            anim.Play("RunR");
        }
        public void Sprint(Animation anim)
        {
            anim.Play("Sprint");
        }
        public void SprintLeft(Animation anim)
        {
            anim.Play("SprintL");
        }
        public void SprintRight(Animation anim)
        {
            anim.Play("SprintR");
        }
    }
}