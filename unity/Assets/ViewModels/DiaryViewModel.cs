 //logic
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Assets.Models;
using Assets.Services;
using UnityEngine;
using Assets.Library;
using UnityEditor.Profiling.Memory.Experimental;
using System;
using System.Diagnostics;
using System.Linq;
using UnityEngine.InputSystem.Android;

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
        // list of all animals: observable collection can identify changes automatically
        public ObservableCollection<Animal> Animals { get; private set; }
   
        // selected diary and entry --> binded to UI
        [SerializeField] public Diary SelectedDiary { get; private set; }
        [SerializeField] public DiaryDiaryentry SelectedConnection { get; private set; }
        [SerializeField] public Diaryentry SelectedDiaryentry { get; private set; }
        // selected user --> binded to UI
        [SerializeField] public User _selectedUser { get; private set; }
        // selected animal --> binded to UI
        public Animal SelectedAnimal { get; private set; }



        public DiaryViewModel()
        {
            _diaryService = new DiaryService();
            Diaries = new ObservableCollection<Diary>();
            Connections = new ObservableCollection<DiaryDiaryentry>();
            Diaryentries = new ObservableCollection<Diaryentry>();
            _selectedUser = SessionData.CurrentUser;
            Animals = new ObservableCollection<Animal>();
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
            var connections = await _diaryService.GetById(SelectedDiary.id); // SD.id STIMMT, ABER SERVICE WIRD NICHT AUFGERUFEN
            foreach (var connection in connections)
            {
                Connections.Add(connection);
            }
            if (Connections.Count > 0)
            {
                SelectedConnection = Connections[0];
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
            if (Diaryentries.Count > 0)
            {
                SelectedDiaryentry = Diaryentries[0];
            }
        }

        // load related animals --> C# async: load but dont block
        public async Task LoadRelatedAnimalsAsync()
        {
            UnityEngine.Debug.Log("Details zu Tieren können derzeit noch nicht angezeigt werden. Wir bitten um Verständnis");
            // get animals by AnimalViewModel
             var animalViewModel = new AnimalViewModel();
             
             // get every Animal and store in list
             foreach (var entry in Diaryentries) {
                var anim = await animalViewModel.LoadAnimalByIdAsync(entry.id_animal);
                UnityEngine.Debug.Log(anim);
                Animals.Add(anim);
             }
             
             // select animal --> first --> change with scroll
            if (Animals.Count > 0)
            {
                SelectedAnimal = Animals[0];
            }
        }

        public async Task Scroll(int counter)
        {
            // change with scroll --> 
            if (counter <= Animals.Count)
            {
                SelectedConnection = Connections[counter];
                SelectedDiaryentry = Diaryentries[counter];
                SelectedAnimal = Animals[counter];
            }
        }
    }
}
