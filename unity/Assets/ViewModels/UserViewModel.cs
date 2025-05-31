//logic
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Assets.Models;
using Assets.Services;
using UnityEngine;
using Assets.Library;

namespace Assets.ViewModels
{
    public class UserViewModel
    {
        // connection to model (--> services)
        private readonly UserService _userService;
        // list of all animals: observable collection can identify changes automatically
        public ObservableCollection<User> Users { get; private set; }
        // selected animal --> binded to UI
        public User SelectedUser { get; private set; }

        public UserViewModel()
        {
            _userService = new UserService();
            Users = new ObservableCollection<User>();
        }

        // SPÄTER IN GETUSERBYUSERNAME ÄNDERN
        // load all users --> C# async: load but dont block
        public async Task LoadAllUsersAsync()
        {
            Users.Clear();
            // call service (service calls api)
            var users = await _userService.GetUsersAsync();
            foreach (var user in users)
            {
                Users.Add(user);
            }

            // OBSOLET WENN USERBYUSERNAME
            if (Users.Count > 0)
            {
                SelectedUser = Users[0];
            }
        }

        // HIER ÜBERPRÜFUNG, OB USER UND PASSWORT KORREKT SIND
        // FEHLERMELDUNG WENN NICHT

        // IN DIESER DATEI AUCH MÖGLICHKEIT ZUR REGISTRIERUNG BIETEN
    }
}
