// classes

using System;
using UnityEngine;

namespace Assets.Models
{
    // user class, serializable for databinding
    [Serializable]
    public class Animal
    {
        // attributes
        [SerializeField] public string id;
        [SerializeField] public string name;
        [SerializeField] public string animationlink;
        [SerializeField] public string habitat;

        // events for databinding
        public event Action<string> OnIdChanged;
        public event Action<string> OnNameChanged;
        public event Action<string> OnAnimationlinkChanged;
        public event Action<string> OnHabitatChanged;

        // getter, setter
        public string Id
        {
            get => id;
            set
            {
                if (id != value)
                {
                    id = value;
                    OnIdChanged?.Invoke(id);
                }
            }
        }
        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnNameChanged?.Invoke(name);
                }
            }
        }
        public string Animationlink
        {
            get => animationlink;
            set
            {
                if (animationlink != value)
                {
                    animationlink = value;
                    OnAnimationlinkChanged?.Invoke(animationlink);
                }
            }
        }
        public string Habitat
        {
            get => habitat;
            set
            {
                if (habitat != value)
                {
                    habitat = value;
                    OnHabitatChanged?.Invoke(habitat);
                }
            }
        }

    }
}
