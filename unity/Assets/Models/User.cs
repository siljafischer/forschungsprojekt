// classes

using System;
using UnityEngine;

namespace Assets.Models
{
    // user class, serializable for databinding
    [Serializable]
    public class User : BusinessObject
    {
        // attributes
        [SerializeField] public string name;
        [SerializeField] public string username;
        [SerializeField] public string mail;
        [SerializeField] public string password;

        // events for databinding
        public event Action<string> OnIdChanged;
        public event Action<string> OnNameChanged;
        public event Action<string> OnUsernameChanged;
        public event Action<string> OnMailChanged;
        public event Action<string> OnPasswordChanged;

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
        public string Username
        {
            get => username;
            set
            {
                if (username != value)
                {
                    username = value;
                    OnUsernameChanged?.Invoke(username);
                }
            }
        }
        public string Mail
        {
            get => mail;
            set
            {
                if (mail != value)
                {
                    mail = value;
                    OnMailChanged?.Invoke(mail);
                }
            }
        }
        public string Password
        {
            get => password;
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPasswordChanged?.Invoke(password);
                }
            }
        }

    }
}
