// get data from API-Endpoints
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Assets.Models;
using UnityEngine;
using Newtonsoft.Json;

namespace Assets.Services
{
    public class DiaryService : MainService
    {

        // wrapper class --> json to usable objects
        [System.Serializable]
        public class DiaryListWrapper
        {
            public string id;
            public string user;
        };

        [Serializable]
        public class ConnectionListWrapper
        {
            public List<DiaryDiaryentry> connections;
        };

        [System.Serializable]
        public class EntryListWrapper
        {
            public string id;
            public string id_animal;
        };

        // get diary by current User
        public async Task<List<Diary>> GetByUser(string user)
        {
            try
            {
                // GET request an die API
                var response = await _httpClient.GetAsync($"Diary/allByUser/{user}");

                if (response.IsSuccessStatusCode)
                {
                    // json-answer
                    var json = await response.Content.ReadAsStringAsync();
                    // into usable objects
                    DiaryListWrapper wrapper = JsonUtility.FromJson<DiaryListWrapper>(json);
                    var diary = new Diary();
                    diary.id = wrapper.id;
                    diary.user = wrapper.user;
                    return new List<Diary> { diary };
                }
                else
                {
                    Debug.Log("Keine Entdeckertagebücher gefunden");
                    return new List<Diary>();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Fehler: {ex.Message}");
                return new List<Diary>();
            }
        }


        // get connections by current Diay
        public async Task<List<DiaryDiaryentry>> GetById(string id)
        {
            try
            {
                // GET request an die API
                var response = await _httpClient.GetAsync($"Diary/allRelatedEntries/{id}");

                if (response.IsSuccessStatusCode)
                {
                    // json-answer
                    var json = await response.Content.ReadAsStringAsync();
                    string wrappedJson = "{\"entries\":" + json + "}";
                    // into usable objects
                    var connections = JsonConvert.DeserializeObject<List<DiaryDiaryentry>>(json);
                    return connections ?? new List<DiaryDiaryentry>();
                }
                else
                {
                    Debug.Log("Keine Tiere gefunden");
                    return new List<DiaryDiaryentry>();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Fehler: {ex.Message}");
                return new List<DiaryDiaryentry>();
            }
        }

        // get entries by current connections
        public async Task<List<Diaryentry>> GetDiaryEntries(string id)
        {
            try
            {
                // GET request an die API
                var response = await _httpClient.GetAsync($"Diaryentry/allById/{id}");

                if (response.IsSuccessStatusCode)
                {
                    // json-answer
                    var json = await response.Content.ReadAsStringAsync();
                    //string wrappedJson = "{\"entries\":" + json + "}";
                    // into usable objects
                    EntryListWrapper wrapper = JsonUtility.FromJson<EntryListWrapper>(json);
                    var entry = new Diaryentry();
                    entry.id = wrapper.id;
                    entry.id_animal = wrapper.id_animal;
                    return new List<Diaryentry> { entry };
                }
                else
                {
                    Debug.Log("Keine Einträge gefunden");
                    return new List<Diaryentry>();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Fehler: {ex.Message}");
                return new List<Diaryentry>();
            }
        }
    }
}
