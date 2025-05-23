// Connection to database (simulation)
// use own models
using System.Globalization;
// get data via linq
using System.Linq;
using backend.Models;


namespace backend.Repositories
{
    public class ElementRoadRepository
    {
        // _ = List
        private readonly List<ElementRoad> _items = new();
        private readonly string _csvFilePath = "db_sim_element_road.csv";
        private bool _initialized = false;

        public ElementRoadRepository()
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

                    System.Diagnostics.Debug.WriteLine($"Daten: {values[0]} {values[1]} {values[2]}");

                    _items.Add(new ElementRoad
                    {
                        id_road = values[0],
                        id_element = values[1],
                        position = values[2]
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
            var lines = new List<string> { "id_road;id_element;position" };
            // values
            lines.AddRange(_items.Select(item => $"{item.id_road};{item.id_element};{item.position}"));
            // save
            File.WriteAllLines(_csvFilePath, lines);
        }


        // get by road
        public ElementRoad GetByRoad(string rid) => _items.FirstOrDefault(item => item.id_road == rid);



        // Create
        public void Create(ElementRoad item)
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

        // Delete by Element
        public void DeleteByElement(string eid)
        {
            _items.RemoveAll(item => item.id_element == eid);
            // save current status
            SaveDataToCsv();

        }
    }
}
