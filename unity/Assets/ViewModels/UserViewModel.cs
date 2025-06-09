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
        [SerializeField] public User _selectedUser { get; private set; }

        // INotifyBindablePropertyChanged implementation
        public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;

        // variables for login
        private string _Username = "";
        private string _Password = "";


        // Properties for login and account --> UI can use this Variables: updated with changes in UI
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
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser != value)
                {
                    // unsubscribe from user
                    if (_selectedUser != null)
                    {
                        UnsubscribeFromUser(_selectedUser);
                    }

                    _selectedUser = value;

                    // subscribe to user
                    if (_selectedUser != null)
                    {
                        SubscribeToUser(_selectedUser);
                    }

                    NotifyPropertyChanged(nameof(SelectedUser));
                    NotifyPropertyChanged(nameof(UserName));
                    NotifyPropertyChanged(nameof(UserUsername));
                    NotifyPropertyChanged(nameof(UserMail));
                    NotifyPropertyChanged(nameof(UserPassword));
                }
            }
        }
        // wrapper-properties for binding --> direct to SelectedUser
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
            SelectedUser = SessionData.CurrentUser;
        }

        // load specific users --> C# async: load but dont block
        public async Task LoadUserByUsernameAsync()
        {

            Users.Clear();

            // call service (service calls api)
            // username gets automatically updated: user input OR use CurrentUser
            var uname = string.IsNullOrWhiteSpace(Username)
                ? SessionData.CurrentUser?.Username
                : Username;

            var users = await _userService.GetUserByUsername(uname);
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
        // update user
        public async Task UpdateUserAsync()
        {
            // update user
            await _userService.UpdateUserAsync(SelectedUser);
            // set updated user as current user for all other scenes
            SyncToSessionUser();
        }
        private void SyncToSessionUser()
        {
            var target = SessionData.CurrentUser;
            var source = SelectedUser;

            if (target != null && source != null)
            {
                target.Name = source.Name;
                target.Username = source.Username;
                target.Mail = source.Mail;
                target.Password = source.Password;
            }
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
