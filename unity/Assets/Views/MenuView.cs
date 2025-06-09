using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.ViewModels;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using Assets.Models;
using TMPro;
using Unity.Properties;
using UnityEditor.UIElements;
using static UnityEditor.Profiling.HierarchyFrameDataView;
using System;

// gets UIDocument-Component 
[RequireComponent(typeof(UIDocument))]
public class MenuView : MonoBehaviour
{
    // connection to viewmodel (View Model contains Logic)
    private UserViewModel _viewModel;

    public void OnRoadPressed()
    {
        Debug.Log("Aktuell steht nur unsere Bauernhofstrecke zur Verfügung");
        SceneManager.LoadScene("BauernhofScene");
    }
    public void OnAccountPressed()
    {
        SceneManager.LoadScene("AccountScene");
    }
    public void OnDiaryPressed()
    {
        Debug.Log("Dieser Bereich steht aktuell noch nicht zur Verfügung");
    }
    public void OnLogoutPressed()
    {
        // delete user data
        SessionData.Clear();
        SceneManager.LoadScene("LoginScene");
    }
}
