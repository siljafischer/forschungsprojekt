using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.ViewModels;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using Assets.Models;
using TMPro;
using Unity.Properties;
using System;

// gets UIDocument-Component 
[RequireComponent(typeof(UIDocument))]
public class MenuView : MonoBehaviour
{
    // connection to viewmodel (View Model contains Logic)
    private UserViewModel _viewModel;

    public void OnRoadPressed()
    {
        Debug.Log("Aktuell steht nur unsere Waldstrecke zur Verfügung");
        SceneManager.LoadScene("ForrestScene");
    }
    public void OnAccountPressed()
    {
        SceneManager.LoadScene("AccountScene");
    }
    public void OnDiaryPressed()
    {
        SceneManager.LoadScene("DiaryScene");
    }
    public void OnLogoutPressed()
    {
        // delete user data
        SessionData.Clear();
        SceneManager.LoadScene("LoginScene");
    }
}
