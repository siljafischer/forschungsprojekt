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
using UnityEngine.UI;

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
    int counter = 0;
    // TextInputs
    [Header("UI Input Fields")]
    public TMP_InputField AnimalNameInput;
    public TMP_InputField DateInput;
    public TMP_InputField HabitatInput;

    // Pictures
    public UnityEngine.UI.Image RealisticPicture;
    public UnityEngine.UI.Image TakenPicture;
    public UnityEngine.UI.Image leftSide;
    public UnityEngine.UI.Image rightSide;

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

        Texture2D photoTexture = Resources.Load<Texture2D>("Photo/pergament");
        if (photoTexture != null)
        {
            Sprite photoSprite = Sprite.Create(photoTexture,
                new Rect(0, 0, photoTexture.width, photoTexture.height),
                Vector2.zero);
            leftSide.sprite = photoSprite;
            rightSide.sprite = photoSprite;
        }

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
            AnimalNameInput.text = "Du hast dieses Tier fotografiert: " + currentAnimal.Name;
            HabitatInput.text = "Hier lebt es: " + currentAnimal.Habitat;
            Texture2D photoTexture = Resources.Load<Texture2D>(currentAnimal.Picture);
            if (photoTexture != null)
            {
                Sprite photoSprite = Sprite.Create(photoTexture,
                    new Rect(0, 0, photoTexture.width, photoTexture.height),
                    Vector2.zero);
                RealisticPicture.sprite = photoSprite;
            }
        }

        if (currentConnection != null)
        {
            Texture2D photoTexture = Resources.Load<Texture2D>(currentConnection.TakenPicture);
            if (photoTexture != null)
            {
                Sprite photoSprite = Sprite.Create(photoTexture,
                    new Rect(0, 0, photoTexture.width, photoTexture.height),
                    Vector2.zero);
                TakenPicture.sprite = photoSprite;
            }
            DateInput.text = "Fotografiert am: " + currentConnection.Date;
        }

        yield return "success";
    }


    // scroll button forward
    public void OnScrollForwardPressed()
    {
        if (counter+1 < _viewModel.Animals.Count)
        {
            counter = counter + 1;
            _viewModel.ScrollForward(counter);

            // update values
            StartCoroutine(FillInputFields());
        }
    }

    // scroll button backward
    public void OnScrollBackwardPressed()
    {
        if (counter-1 >= 0)
        {
            counter = counter - 1;
            _viewModel.ScrollBackward(counter);

            // update values
            StartCoroutine(FillInputFields());
        }
    }

    // back button
    public void OnBackPressed()
    {
        // back to login
        SceneManager.LoadScene("MenuScene");
    }

}