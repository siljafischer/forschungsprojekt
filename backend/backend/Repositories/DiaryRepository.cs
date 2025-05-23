// Connection to database (simulation)
// use own models
using System.Globalization;
// get data via linq
using System.Linq;
using backend.Models;


namespace backend.Repositories
{
    public class DiaryRepository
    {
        // _ = List
        private readonly List<Diary> _items = new();
        private readonly string _csvFilePath = "db_sim_diary.csv";
        private bool _initialized = false;

        public DiaryRepository()
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

                    System.Diagnostics.Debug.WriteLine($"Daten: {values[0]} {values[1]}");

                    _items.Add(new Diary
                    {
                        id = values[0],
                        user = values[1]
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
            var lines = new List<string> { "id;user" };
            // values
            lines.AddRange(_items.Select(item => $"{item.id};{item.user}"));
            // save
            File.WriteAllLines(_csvFilePath, lines);
        }

        // get all : LINQ
        public IEnumerable<Diary> GetAll() => _items;

        // get first with id (only one per id)
        public Diary GetById(string id) => _items.FirstOrDefault(item => item.id == id);

        // get first with user (only one per id)
        public Diary GetByUser(string user) => _items.FirstOrDefault(item => item.user == user);


        // Create
        public void Create(Diary item)
        {
            _items.Add(item);
            // save current status
            SaveDataToCsv();
        }


        // Update by id
        public void Update(Diary item)
        {
            var existingItem = _items.FirstOrDefault(i => i.id == item.id);
            if (existingItem != null)
            {
                existingItem.id = item.id;
                existingItem.user = item.user;
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
