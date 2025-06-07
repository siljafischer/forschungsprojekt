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
using static UnityEditor.Profiling.HierarchyFrameDataView;

namespace Assets.ViewModels
{
    public class UserViewModel : INotifyBindablePropertyChanged
    {

        // connection to model (--> services)
        private readonly UserService _userService;
        // list of all users: observable collection can identify changes automatically
        public ObservableCollection<User> Users { get; private set; }
        // selected user --> binded to UI
        [SerializeField] public User SelectedUser { get; private set; }

        // INotifyBindablePropertyChanged implementation
        public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;

        // variables as result of login
        private string _Name = "";
        private string _Username = "";
        private string _Mail = "";
        private string _Password = "";


        // Properties for login and account --> UI can use this Variables: updated with changes in UI
        public string Name
        {
            get => _Name;
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        public string Username
        {
            get => _Username;
            set
            {
                if (_Username != value)
                {
                    _Username = value;
                    NotifyPropertyChanged(nameof(Username));
                }
            }
        }
        public string Mail
        {
            get => _Mail;
            set
            {
                if (_Mail != value)
                {
                    _Mail = value;
                    NotifyPropertyChanged(nameof(Mail));
                }
            }
        }
        public string Password
        {
            get => _Password;
            set
            {
                if (_Password != value)
                {
                    _Password = value;
                    NotifyPropertyChanged(nameof(Password));
                }
            }
        }

        // selectedUser == current user --> automatic updates if user changed in UI <-> Model
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
                    NotifyPropertyChanged(nameof(UserName));
                    NotifyPropertyChanged(nameof(UserUsername));
                    NotifyPropertyChanged(nameof(UserMail));
                    NotifyPropertyChanged(nameof(UserPassword));
                }
            }
        }
        // wrapper-properties for binding
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
            // username gets automatically updated: user input
            var users = await _userService.GetUserByUsername(Username);
            foreach (var user in users)
            {
                Users.Add(user);
            }
            // Authenticate
            if (Users[0].password == Password)
            {
                // set selectedUser
                SelectedUser = Users[0];
                // set logged user as current user for all other scenes
                SessionData.CurrentUser = SelectedUser;
            }
            else
            {
                Debug.LogError("Falsches Passwort");
            }
        }


        // IN DIESER DATEI AUCH MÖGLICHKEIT ZUR REGISTRIERUNG BIETEN
        // safe user
        public async Task UpdateUserAsync()
        {
            Debug.LogError($"Hi {SelectedUser.name}, aktuell werden noch keine Änderungen gespeichert.\nFolgende Änderungen sind vorgemerkt");
            Debug.Log($"{_Name} {_Username} {_Mail} {_Password}");
            //await _userService.UpdateUserAsync(SelectedUser);
        }


        // gets updates from model --> ViewModel can publish changes to UI --> UI gets updated
        private void SubscribeToUser(User user)
        {
            user.OnNameChanged += OnUserNameChanged;
            user.OnUsernameChanged += OnUserUsernameChanged;
            user.OnMailChanged += OnUserMailChanged;
            user.OnPasswordChanged += OnUserPasswordChanged;
        }
        private void UnsubscribeFromUser(User user)
        {
            user.OnNameChanged -= OnUserNameChanged;
            user.OnUsernameChanged -= OnUserUsernameChanged;
            user.OnMailChanged -= OnUserMailChanged;
            user.OnPasswordChanged -= OnUserPasswordChanged;
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

        // connects ViewModel <-> UI --> informs about changes in property
        private void NotifyPropertyChanged(string propertyName)
        {
            propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(propertyName));
        }
    }
}
