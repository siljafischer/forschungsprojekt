// Connection to database (simulation)
// use own models
using System.Globalization;
// get data via linq
using System.Linq;
using backend.Models;


namespace backend.Repositories
{
    public class AnimalRoadRepository
    {
        // _ = List
        private readonly List<AnimalRoad> _items = new();
        private readonly string _csvFilePath = "db_sim_animal_road.csv";
        private bool _initialized = false;

        public AnimalRoadRepository()
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

                    System.Diagnostics.Debug.WriteLine($"Daten: {values[0]} {values[1]} {values[2]} {values[3]} {values[4]} {values[5]}");

                    _items.Add(new AnimalRoad
                    {
                        id_road = values[0],
                        id_animal = values[1],
                        position_a = values[2],
                        position_b = values[3],
                        escape_radius = values[4],
                        duration = values[5],
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
            var lines = new List<string> { "id_road;id_animal;position_a;position_b;escape_radius;duration" };
            // values
            lines.AddRange(_items.Select(item => $"{item.id_road};{item.id_animal};{item.position_a};{item.position_b};{item.escape_radius};{item.duration}"));
            // save
            File.WriteAllLines(_csvFilePath, lines);
        }


        // get by Road
        public AnimalRoad GetByRoad(string rid) => _items.FirstOrDefault(item => item.id_road == rid);


        // Create
        public void Create(AnimalRoad item)
        {
            _items.Add(item);
            // save current status
            SaveDataToCsv();
        }

        // Delete by Road
        public void DeleteByRoad(string rid)
        {
            _items.RemoveAll(item => item.id_road == rid);
            // save current status
            SaveDataToCsv();

        }

        // Delete by Animal
        public void DeleteByAnimal(string aid)
        {
            _items.RemoveAll(item => item.id_animal == aid);
            // save current status
            SaveDataToCsv();

        }
    }
}
