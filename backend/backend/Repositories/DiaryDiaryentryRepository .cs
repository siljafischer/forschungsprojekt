// Connection to database (simulation)
// use own models
using System.Globalization;
// get data via linq
using System.Linq;
using backend.Models;


namespace backend.Repositories
{
    public class DiaryDiaryentryRepository
    {
        // _ = List
        private readonly List<DiaryDiaryentry> _items = new();
        private readonly string _csvFilePath = "db_sim_diary_diaryentry.csv";
        private bool _initialized = false;

        public DiaryDiaryentryRepository()
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

                    _items.Add(new DiaryDiaryentry
                    {
                        id_diary = values[0],
                        id_diaryentry = values[1],
                        date = values[2],
                        taken_picture = values[3]
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
            var lines = new List<string> { "id_diary;id_diaryentry;date;taken_picture" };
            // values
            lines.AddRange(_items.Select(item => $"{item.id_diary};{item.id_diaryentry};{item.date};{item.taken_picture}"));
            // save
            File.WriteAllLines(_csvFilePath, lines);
        }


        // get by diary
        public DiaryDiaryentry GetByDiary(string did) => _items.FirstOrDefault(item => item.id_diary == did);



        // Create
        public void Create(DiaryDiaryentry item)
        {
            _items.Add(item);
            // save current status
            SaveDataToCsv();
        }

        // Delete by Diary_id
        public void DeleteByDiary(string did)
        {
            _items.RemoveAll(item => item.id_diary == did);
            // save current status
            SaveDataToCsv();

        }

        // Delete by Diaryentry_id
        public void DeleteByDiaryentry(string eid)
        {
            _items.RemoveAll(item => item.id_diaryentry == eid);
            // save current status
            SaveDataToCsv();

        }
    }
}
