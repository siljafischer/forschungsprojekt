// classes

using System;
using UnityEngine;

namespace Assets.Models
{
    // user class
    [Serializable]
    public class Diary : BusinessObject
    {
        // attributes
        public string user { get; set; }
    }
}
