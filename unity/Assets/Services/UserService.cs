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
    public class UserService : MainService
    {
        // wrapper class --> json to usable objects
        [Serializable]
        public class UserListWrapper
        {
            public List<User> users;
        }


        // get all Users
        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                // GET request an die API
                var response = await _httpClient.GetAsync("User");

                if (response.IsSuccessStatusCode)
                {
                    // json-answer
                    var json = await response.Content.ReadAsStringAsync();
                    string wrappedJson = "{\"users\":" + json + "}";
                    // into usable objects
                    UserListWrapper wrapper = JsonUtility.FromJson<UserListWrapper>(wrappedJson);

                    return wrapper.users;
                }
                else
                {
                    Debug.Log("Keine User gefunden");
                    return new List<User>();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Fehler: {ex.Message}");
                return new List<User>();
            }
        }


        // get User by Id
        public async Task<List<User>> GetUserByIdAsync(int id)
        {
            try
            {
                // GET
                var response = await _httpClient.GetAsync($"/User/allById/{id}");

                // check if response is correct
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    // work-around: only one item in list
                    var User = JsonUtility.FromJson<User>(json);
                    var z = new List<User> { User };
                    return z;
                }
                else
                {
                    // error: empty list
                    System.Diagnostics.Debug.WriteLine("User nicht gefunden");
                    return new List<User>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler: {ex.Message}");
                return new List<User>();
            }
        }

        // get User by username
        public async Task<List<User>> GetUserByUsernameAsync(string username)
        {
            try
            {
                // GET
                var response = await _httpClient.GetAsync($"User/allByUsername/{username}");

                // check if response is correct
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    // work-around: only one item in list
                    var User = JsonUtility.FromJson<User>(json);
                    var z = new List<User> { User };
                    return z;
                }
                else
                {
                    // error: empty list
                    System.Diagnostics.Debug.WriteLine("User nicht gefunden");
                    return new List<User>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler: {ex.Message}");
                return new List<User>();
            }
        }






        // create new user
        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                // create json
                var json = JsonUtility.ToJson(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // POST --> create new object
                var response = await _httpClient.PostAsync("User/create", content);

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


        //update user
        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                // create json
                var json = JsonUtility.ToJson(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // PUT --> update object
                var response = await _httpClient.PutAsync($"/User/update/{user.id}", content);

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


        // delete user
        public async Task<bool> DeleteUserAsync(string id)
        {
            try
            {
                // DELETE
                var response = await _httpClient.DeleteAsync($"User/delete/{id}");

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
