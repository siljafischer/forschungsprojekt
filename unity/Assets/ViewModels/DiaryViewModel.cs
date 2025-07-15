 //logic
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Assets.Models;
using Assets.Services;
using UnityEngine;
using Assets.Library;
using UnityEditor.Profiling.Memory.Experimental;

namespace Assets.ViewModels
{
    public class DiaryViewModel
    {
        // connection to model (--> services)
        private readonly DiaryService _diaryService;
        // list of all diaries, connections, entries: observable collection can identify changes automatically
        public ObservableCollection<Diary> Diaries { get; private set; }
        public ObservableCollection<DiaryDiaryentry> Connections { get; private set; }
        public ObservableCollection<Diaryentry> Diaryentries { get; private set; }
        // selected diary and entry --> binded to UI
        public Diary SelectedDiary { get; private set; }
        public DiaryDiaryentry SelectedConnection { get; private set; }
        public Diaryentry SelectedDiaryentry { get; private set; }
        // selected user --> binded to UI
        [SerializeField] public User _selectedUser { get; private set; }


        public DiaryViewModel()
        {
            _diaryService = new DiaryService();
            Diaries = new ObservableCollection<Diary>();
            Diaryentries = new ObservableCollection<Diaryentry>();
            _selectedUser = SessionData.CurrentUser;
        }

        // load only diary
        public async Task LoadDiaryAsync()
        {
            // load diary
            Diaries.Clear();
            var diaries = await _diaryService.GetByUser(_selectedUser.Id);
            foreach (var diary in diaries)
            {
                Diaries.Add(diary);
            }
            if (Diaries.Count > 0)
            {
                SelectedDiary = Diaries[0];
            }
        }

        // load all diary --> C# async: load but dont block
        public async Task LoadDiaryAndEntriesAsync()
        {
            // load diary
            Diaries.Clear();
            var diaries = await _diaryService.GetByUser(_selectedUser.Id);
            foreach (var diary in diaries)
            {
                Diaries.Add(diary);
            }
            if (Diaries.Count > 0)
            {
                SelectedDiary = Diaries[0];
            }

            // get connections
            Connections.Clear();
            var connections = await _diaryService.GetById(SelectedDiary.id);
            foreach (var connection in connections)
            {
                Connections.Add(connection);
            }

            // load all related entries
            Diaryentries.Clear();
            foreach (var connection in Connections)
            {
                var entries = await _diaryService.GetDiaryEntries(connection.id_diary);
                foreach (var entry in entries)
                {
                    Diaryentries.Add(entry);
                }
            }
        }

        // load related animals --> C# async: load but dont block
        public async Task LoadRelatedAnimalsAsync()
        {
            Debug.Log("Details zu Tieren können derzeit noch nciht angezeigt werden. Wir bitten um Verständnis");
        }
    }
}
