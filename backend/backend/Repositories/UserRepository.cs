// Connection to database (simulation)
// use own models
using System.Globalization;
// get data via linq
using System.Linq;
using backend.Models;


namespace backend.Repositories
{
    public class UserRepository
    {
        // _ = List
        private readonly List<User> _items = new();
        private readonly string _csvFilePath = "db_sim_user.csv";
        private bool _initialized = false;

        public UserRepository()
        {
            if (!_initialized)
            {
                LoadDataFromCsv();
                _initialized = true;
            }
        }


        // read and save to csv --> current storage
        private void LoadDataFromCsv()
        {
            if (File.Exists(_csvFilePath))
            {
                var lines = File.ReadAllLines(_csvFilePath);

                // skip header
                foreach (var line in lines.Skip(1))
                {
                    var values = line.Split(';');

                    System.Diagnostics.Debug.WriteLine($"Daten: {values[0]} {values[1]} {values[2]} {values[3]} {values[4]}");

                    _items.Add(new User
                    {
                        id = values[0],
                        name = values[1],
                        username = values[2],
                        mail = values[3],
                        password = values[4]
                    });

                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Datei nicht gefunden: {_csvFilePath}");
            }
        }

        private void SaveDataToCsv()
        {
            // header
            var lines = new List<string> { "id;name;username;mail;password" };
            // values
            lines.AddRange(_items.Select(item => $"{item.id};{item.name};{item.username};{item.mail};{item.password}"));
            // save
            File.WriteAllLines(_csvFilePath, lines);
        }


        // get all : LINQ
        public IEnumerable<User> GetAll() => _items;


        // get first with id (only one per id)
        public User GetById(string id) => _items.FirstOrDefault(item => item.id == id);

        // get first with username (only one per username)
        public User GetByUsername(string username) => _items.FirstOrDefault(item => item.username == username);


        // Create
        public void Create(User item)
        {
            _items.Add(item);
            // save current status
            SaveDataToCsv();
        }


        // Update by id
        public void Update(User item)
        {
            var existingItem = _items.FirstOrDefault(i => i.id == item.id);
            if (existingItem != null)
            {
                existingItem.id = item.id;
                existingItem.name = item.name;
                existingItem.username = item.username;
                existingItem.mail = item.mail;
                existingItem.password = item.password;
                // save current status
                SaveDataToCsv();
            }
        }


        // Delete by id
        public void Delete(string id)
        {
            _items.RemoveAll(item => item.id == id);
            // save current status
            SaveDataToCsv();

        }
    }
}
