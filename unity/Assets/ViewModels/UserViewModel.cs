//logic
// HIER VORBEREITUNGEN FÜR ÄNDERUNG IN EINEM FORMULAR --> VIEW MUSS DRINGEND ANGEPASST WERDEN
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Assets.Models;
using Assets.Services;
using UnityEngine;
using Assets.Library;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;

namespace Assets.ViewModels
{
    public class UserViewModel : INotifyBindablePropertyChanged
    {
        // ENTFERNEN, WENN LOGINFORMULAR EINGEFÜGT
        public string uname = "juli";



        // connection to model (--> services)
        private readonly UserService _userService;
        // list of all users: observable collection can identify changes automatically
        public ObservableCollection<User> Users { get; private set; }
        // selected user --> binded to UI
        [SerializeField] public User SelectedUser { get; private set; }

        // INotifyBindablePropertyChanged implementation
        public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;

        // Properties for binding
        public User SelectedUSer
        {
            get => SelectedUser;
            set
            {
                if (SelectedUser != value)
                {
                    // unsubscribe from user
                    if (SelectedUser != null)
                    {
                        UnsubscribeFromUser(SelectedUser);
                    }

                    SelectedUser = value;

                    // subscribe to user
                    if (SelectedUser != null)
                    {
                        SubscribeToUser(SelectedUser);
                    }

                    NotifyPropertyChanged(nameof(SelectedUser));
                    NotifyPropertyChanged(nameof(UserId));
                    NotifyPropertyChanged(nameof(UserName));
                    NotifyPropertyChanged(nameof(UserUsername));
                    NotifyPropertyChanged(nameof(UserMail));
                    NotifyPropertyChanged(nameof(UserPassword));
                }
            }
        }
        // wrapper-properties for binding
        public string UserId
        {
            get => SelectedUser?.Id ?? "";
            set
            {
                if (SelectedUser != null && SelectedUser.Id != value)
                {
                    SelectedUser.Id = value;
                    NotifyPropertyChanged(nameof(UserId));
                }
            }
        }
        public string UserName
        {
            get => SelectedUser?.Name ?? "";
            set
            {
                if (SelectedUser != null && SelectedUser.Name != value)
                {
                    SelectedUser.Name = value;
                    NotifyPropertyChanged(nameof(UserName));
                }
            }
        }
        public string UserUsername
        {
            get => SelectedUser?.Username ?? "";
            set
            {
                if (SelectedUser != null && SelectedUser.Username != value)
                {
                    SelectedUser.Username = value;
                    NotifyPropertyChanged(nameof(UserUsername));
                }
            }
        }
        public string UserMail
        {
            get => SelectedUser?.Mail ?? "";
            set
            {
                if (SelectedUser != null && SelectedUser.Mail != value)
                {
                    SelectedUser.Mail = value;
                    NotifyPropertyChanged(nameof(UserMail));
                }
            }
        }
        public string UserPassword
        {
            get => SelectedUser?.Password ?? "";
            set
            {
                if (SelectedUser != null && SelectedUser.Password != value)
                {
                    SelectedUser.Password = value;
                    NotifyPropertyChanged(nameof(UserPassword));
                }
            }
        }

        public UserViewModel()
        {
            _userService = new UserService();
            Users = new ObservableCollection<User>();
        }

        // load specific users --> C# async: load but dont block
        public async Task LoadUserByUsernameAsync()
        {
            Users.Clear();
            // call service (service calls api)
            var users = await _userService.GetUserByUsername(uname);
            foreach (var user in users)
            {
                Users.Add(user);
            }

            if (Users.Count > 0)
            {
                SelectedUser = Users[0];
            }
        }

        // HIER ÜBERPRÜFUNG, OB USER UND PASSWORT KORREKT SIND
        // FEHLERMELDUNG WENN NICHT

        // IN DIESER DATEI AUCH MÖGLICHKEIT ZUR REGISTRIERUNG BIETEN


        // Bindingstuff
        private void SubscribeToUser(User user)
        {
            user.OnIdChanged += OnUserIdChanged;
            user.OnNameChanged += OnUserNameChanged;
            user.OnUsernameChanged += OnUserUsernameChanged;
            user.OnMailChanged += OnUserMailChanged;
            user.OnPasswordChanged += OnUserPasswordChanged;
        }
        private void UnsubscribeFromUser(User user)
        {
            user.OnIdChanged -= OnUserIdChanged;
            user.OnNameChanged -= OnUserNameChanged;
            user.OnUsernameChanged -= OnUserUsernameChanged;
            user.OnMailChanged -= OnUserMailChanged;
            user.OnPasswordChanged -= OnUserPasswordChanged;
        }

        private void OnUserIdChanged(string newId)
        {
            NotifyPropertyChanged(nameof(UserId));
        }

        private void OnUserNameChanged(string newName)
        {
            NotifyPropertyChanged(nameof(UserName));
        }

        private void OnUserUsernameChanged(string newUsername)
        {
            NotifyPropertyChanged(nameof(UserName));
        }

        private void OnUserMailChanged(string newMail)
        {
            NotifyPropertyChanged(nameof(UserMail));
        }
        private void OnUserPasswordChanged(string newPassword)
        {
            NotifyPropertyChanged(nameof(UserMail));
        }
        public async void SaveUser()
        {
            if (SelectedUser != null)
            {
                await _userService.UpdateUserAsync(SelectedUser);
            }
        }
        private void NotifyPropertyChanged(string propertyName)
        {
            propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(propertyName));
        }
    }
}
