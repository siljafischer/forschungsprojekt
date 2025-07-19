// get data from API-Endpoints
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Assets.Models;
using UnityEngine;

namespace Assets.Services
{
    public class DiaryService : MainService
    {

        // wrapper class --> json to usable objects
        [Serializable]
        public class DiaryListWrapper
        {
            public List<Diary> diaries;
        };

        [Serializable]
        public class ConnectionListWrapper
        {
            public List<DiaryDiaryentry> connections;
        };

        [Serializable]
        public class EntryListWrapper
        {
            public List<Diaryentry> entries;
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
                    string wrappedJson = "{\"diaries\":" + json + "}";
                    // into usable objects
                    DiaryListWrapper wrapper = JsonUtility.FromJson<DiaryListWrapper>(wrappedJson);
                    Debug.Log(wrapper.diaries);

                    return wrapper.diaries ;
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
                var response = await _httpClient.GetAsync($"Diaryentry/allById/{id}");

                if (response.IsSuccessStatusCode)
                {
                    // json-answer
                    var json = await response.Content.ReadAsStringAsync();
                    string wrappedJson = "{\"entries\":" + json + "}";
                    // into usable objects
                    ConnectionListWrapper wrapper = JsonUtility.FromJson<ConnectionListWrapper>(wrappedJson);

                    return wrapper.connections;
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
                var response = await _httpClient.GetAsync($"Diary/allRelatedEntries/{id}");

                if (response.IsSuccessStatusCode)
                {
                    // json-answer
                    var json = await response.Content.ReadAsStringAsync();
                    string wrappedJson = "{\"entries\":" + json + "}";
                    // into usable objects
                    EntryListWrapper wrapper = JsonUtility.FromJson<EntryListWrapper>(wrappedJson);

                    return wrapper.entries;
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
