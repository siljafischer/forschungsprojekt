// classes

using System;

namespace Assets.Models
{
    // user class
    [Serializable]
    public class Animal : BusinessObject
    {
        // attributes
        public string name;
        public string animationlink;
        public string habitat;
    }
}
