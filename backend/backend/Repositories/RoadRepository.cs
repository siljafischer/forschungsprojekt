// Connection to database (simulation)
// use own models
using System.Globalization;
// get data via linq
using System.Linq;
using backend.Models;


namespace backend.Repositories
{
    public class RoadRepository
    {
        // _ = List
        private readonly List<Animal> _items = new();
        private readonly string _csvFilePath = "db_sim_road.csv";
        private bool _initialized = false;

        public RoadRepository()
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

                    System.Diagnostics.Debug.WriteLine($"Daten: {values[0]} {values[1]} {values[2]} {values[3]}");

                    _items.Add(new Animal
                    {
                        id = values[0],
                        name = values[1],
                        animationlink = values[2],
                        habitat = values[3]
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
            var lines = new List<string> { "id;name;animationlink;habitat" };
            // values
            lines.AddRange(_items.Select(item => $"{item.id};{item.name};{item.animationlink};{item.habitat}"));
            // save
            File.WriteAllLines(_csvFilePath, lines);
        }


        // get all : LINQ
        public IEnumerable<Animal> GetAll() => _items;


        // get first with id (only one per id)
        public Animal GetById(string id) => _items.FirstOrDefault(item => item.id == id);


        // Create
        public void Create(Animal item)
        {
            _items.Add(item);
            // save current status
            SaveDataToCsv();
        }


        // Update by id
        public void Update(Animal item)
        {
            var existingItem = _items.FirstOrDefault(i => i.id == item.id);
            if (existingItem != null)
            {
                existingItem.id = item.id;
                existingItem.name = item.name;
                existingItem.animationlink = item.animationlink;
                existingItem.habitat = item.habitat;
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
