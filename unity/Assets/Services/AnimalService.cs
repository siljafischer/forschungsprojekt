// get data from API-Endpoints
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Assets.Models;
using UnityEngine;

namespace Assets.Services
{
    public class AnimalService
    {
        private readonly HttpClient _httpClient;

        // Base URL
        public AnimalService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7254")
            };
            Debug.Log($"HttpClient erstellt: {_httpClient.BaseAddress}");
        }

        // Wrapper-Klasse, um das JSON-Array als Objekt zu behandeln
        [System.Serializable]
        public class AnimalListWrapper
        {
            public List<Animal> animals;
        }

        // get all Animals
        public async Task<List<Animal>> GetAnimalsAsync()
        {
            try
            {
                // GET request an die API
                var response = await _httpClient.GetAsync("Animal");

                // Überprüfen, ob die Antwort erfolgreich war
                if (response.IsSuccessStatusCode)
                {
                    // Hole die JSON-Antwort als String
                    var json = await response.Content.ReadAsStringAsync();

                    // Hier verpacken wir das JSON-Array in ein JSON-Objekt für JsonUtility
                    // HIER LIEGT DAS PROBLEM!!!
                    var wrapper = JsonUtility.FromJson<AnimalListWrapper>("{\"animals\":" + json + "}");
                    Debug.Log(wrapper.animals);

                    // Rückgabe der Liste von Tieren
                    return wrapper.animals;
                }
                else
                {
                    // Fehler: Leere Liste zurückgeben
                    Debug.Log("Keine Tiere gefunden");
                    return new List<Animal>();
                }
            }
            catch (Exception ex)
            {
                // Fehler beim Abrufen der Daten
                Debug.LogError($"Fehler: {ex.Message}");
                return new List<Animal>();
            }
        }


        // get Animal by Id
        public async Task<List<Animal>> GetAnimalById(int id)
        {
            try
            {
                // GET
                var response = await _httpClient.GetAsync($"/Animal/allById/{id}");

                // check if response is correct
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    // work-around: only one item in list
                    var Animal = JsonUtility.FromJson<Animal>(json);
                    var z = new List<Animal> { Animal };
                    return z;
                }
                else
                {
                    // error: empty list
                    System.Diagnostics.Debug.WriteLine("Keine Kalender gefunden");
                    return new List<Animal>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler: {ex.Message}");
                return new List<Animal>();
            }
        }


        


        // create new animal
        public async Task<bool> CreateAnimalAsync(Animal animal)
        {
            try
            {
                // create json
                var json = JsonUtility.ToJson(animal);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // POST --> create new object
                var response = await _httpClient.PostAsync("Animal/create", content);

                // check if writing was succesful
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    
                    var errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Fehler: {response.StatusCode} - {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler: {ex.Message}");
                return false;
            }
        }


        //update animal
        public async Task<bool> UpdateAnimalAsync(Animal animal)
        {
            try
            {
                // create json
                var json = JsonUtility.ToJson(animal);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // PUT --> update object
                var response = await _httpClient.PutAsync($"/Animal/update/{animal.id}", content);

                // check if writing was succesful
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Fehler: {response.StatusCode} - {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler: {ex.Message}");
                return false;
            }
        }


        // delete animal
        public async Task<bool> DeleteAnimalAsync(string id)
        {
            try
            {
                // DELETE
                var response = await _httpClient.DeleteAsync($"Animal/delete/{id}");

                // check if was succesful
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Fehler: {response.StatusCode} - {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler: {ex.Message}");
                return false;
            }
        }
    }
}
