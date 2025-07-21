// SEITE MIT BUTTON NÄCHSTE SEITE" --> HOLT EINFACH IMMER NÄCHSTES ELEMENT AUS EINER LISTE :--)
using UnityEngine;
using Assets.ViewModels;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using Assets.Models;
using UnityEngine.SceneManagement;
using System;

// gets UIDocument-Component 
[RequireComponent(typeof(UIDocument))]
public class DiaryentryView : MonoBehaviour
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
        Debug.Log(CurrentUser.Name + ", schau dir an, welche Tiere du schon fotografiert hast!");

        // load animals async (Coroutine ~ async/await: wait but dont block game)
        StartCoroutine(LoadAndDisplayEntries());
    }

    private IEnumerator LoadAndDisplayEntries()
    {
        // load connections and entries and wait until loaded
        Task loadTask = _viewModel.LoadDiaryAndEntriesAsync();
        // called by Coroutine --> WaitUntil ~ async/await: wait until task ist completed)
        yield return new WaitUntil(() => loadTask.IsCompleted);

        if (_viewModel.Diaryentries == null || _viewModel.Diaryentries.Count == 0)
        {
            Debug.LogWarning("So wie es aussieht, hast du noch keine Tiere fotografiert. Hier ein Beispieleintrag");
            yield break;
        }

        // load related animals
        Task loadTaskAnimals = _viewModel.LoadRelatedAnimalsAsync();
        // called by Coroutine --> WaitUntil ~ async/await: wait until task ist completed)
        yield return new WaitUntil(() => loadTaskAnimals.IsCompleted);
    }

    // back button
    public void OnBackPressed()
    {
        // back to login
        SceneManager.LoadScene("MenuScene");
    }

}