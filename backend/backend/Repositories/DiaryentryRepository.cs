// Connection to database (simulation)
// use own models
using System.Globalization;
// get data via linq
using System.Linq;
using backend.Models;


namespace backend.Repositories
{
    public class DiaryentryRepository
    {
        // _ = List
        private readonly List<Diaryentry> _items = new();
        private readonly string _csvFilePath = "db_sim_diaryentry.csv";
        private bool _initialized = false;

        public DiaryentryRepository()
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

                    _items.Add(new Diaryentry
                    {
                        id = values[0],
                        id_animal = values[1]
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
            var lines = new List<string> { "id;id_animal" };
            // values
            lines.AddRange(_items.Select(item => $"{item.id};{item.id_animal}"));
            // save
            File.WriteAllLines(_csvFilePath, lines);
        }

        // get all : LINQ
        public IEnumerable<Diaryentry> GetAll() => _items;


        // get first with id (only one per id)
        public Diaryentry GetById(string id) => _items.FirstOrDefault(item => item.id == id);


        // Create
        public void Create(Diaryentry item)
        {
            _items.Add(item);
            // save current status
            SaveDataToCsv();
        }


        // Update by id
        public void Update(Diaryentry item)
        {
            var existingItem = _items.FirstOrDefault(i => i.id == item.id);
            if (existingItem != null)
            {
                existingItem.id = item.id;
                existingItem.id_animal = item.id_animal;
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

        // Delete by animal
        public void DeleteByAnimal(string id)
        {
            _items.RemoveAll(item => item.id_animal == id);
            // save current status
            SaveDataToCsv();

        }
    }
}
