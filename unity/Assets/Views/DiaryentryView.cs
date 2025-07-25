// SEITE MIT BUTTON NÄCHSTE SEITE" --> HOLT EINFACH IMMER NÄCHSTES ELEMENT AUS EINER LISTE :--)
using UnityEngine;
using Assets.ViewModels;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using Assets.Models;
using UnityEngine.SceneManagement;
using System;
using TMPro;

// gets UIDocument-Component 
[RequireComponent(typeof(UIDocument))]
public class DiaryentryView : MonoBehaviour
{
    // connection to viewmodel (View Model contains Logic)
    private DiaryViewModel _viewModel;
    // stores current selected stuff (only one for databinding)
    private GameObject _connectionInstance;
    private GameObject _entryInstance;
    private GameObject _animalInstance;
    // root of UI --> acceess UI (e.g. change text, use buttons)
    [SerializeField] private UIDocument _uiDocument;
    private VisualElement _root;

    // counter for scrolling
    int counter = 1;
    // TextInputs
    [Header("UI Input Fields")]
    public TMP_InputField AnimalNameInput;
    public TMP_InputField DateInput;
    public TMP_InputField HabitatInput;
    public TMP_InputField PictureInput;

    // automatic call: activate object
    private void Awake()
    {
        if (_uiDocument == null)
            _uiDocument = GetComponent<UIDocument>();
    }

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

        // show animals
        StartCoroutine(FillInputFields());
    }

    private IEnumerator FillInputFields()
    {
        // get current Entry and Animal
        var currentConnection = _viewModel.SelectedConnection;
        var currentAnimal = _viewModel.SelectedAnimal;

        if (currentAnimal != null)
        {
            ((TextMeshProUGUI)AnimalNameInput.placeholder).text = "Du hast dieses Tier fotografiert: " + currentAnimal.Name;
            ((TextMeshProUGUI)HabitatInput.placeholder).text = "Hier lebt es: " + currentAnimal.Habitat;
        }

        if (currentConnection != null)
        {
            ((TextMeshProUGUI)PictureInput.placeholder).text = currentConnection.TakenPicture;
            ((TextMeshProUGUI)DateInput.placeholder).text = "Fotografiert am: " + currentConnection.Date;
        }

        yield return "success";
    }


    // scroll button forward
    public void OnScrollForwardPressed()
    {
        _viewModel.ScrollForward(counter);
        counter = counter + 1;

        // update values
        StartCoroutine(FillInputFields());
    }

    // scroll button backward
    public void OnScrollBackwardPressed()
    {
        counter = counter - 1;
        _viewModel.ScrollBackward(counter);

        // update values
        StartCoroutine(FillInputFields());
    }

    // back button
    public void OnBackPressed()
    {
        // back to login
        SceneManager.LoadScene("MenuScene");
    }

}