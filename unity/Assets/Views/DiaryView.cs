// SEITE MIT BUTTON "ÖFFNEN"
using UnityEngine;
using Assets.ViewModels;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using Assets.Models;
using UnityEngine.SceneManagement;

// gets UIDocument-Component 
[RequireComponent(typeof(UIDocument))]
public class DiaryView : MonoBehaviour
{
    // connection to viewmodel (View Model contains Logic)
    private DiaryViewModel _viewModel;
    // root of UI --> acceess UI (e.g. change text, use buttons)
    private VisualElement _root;

    // automatic call: activate object (e.g. gamestart)
    private void Start()
    {
        _viewModel = new DiaryViewModel();

        // get current user from SessionData
        var CurrentUser = SessionData.CurrentUser;
        Debug.Log("Hi " + CurrentUser.Name + ", öffne doch mal dein Tagebuch und schaue, welchen Tieren du begegnest");

        // load animals async (Coroutine ~ async/await: wait but dont block game)
        //StartCoroutine(LoadAndDisplayAnimals());
    }

    // back button
    public void OnOpenPressed()
    {
        // back to login
        SceneManager.LoadScene("DiaryentryScene");
    }

    // back button
    public void OnBackPressed()
    {
        // back to login
        SceneManager.LoadScene("MenuScene");
    }

}