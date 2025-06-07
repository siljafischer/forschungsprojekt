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

// gets UIDocument-Component 
[RequireComponent(typeof(UIDocument))]
public class AccountView : MonoBehaviour
{
    // connection to viewmodel (View Model contains Logic)
    private UserViewModel _viewModel;
    // stores current selected user (only one for databinding)
    private GameObject _userInstance;
    // root of UI --> acceess UI (e.g. change text, use buttons)
    [SerializeField] private UIDocument _uiDocument;
    private VisualElement _root;

    // TextInputs
    [Header("UI Input Fields")]
    public TMP_InputField NameInput;
    public TMP_InputField UserNameInput;
    public TMP_InputField MailInput;
    public TMP_InputField PasswordInput;

    // automatic call: activate object
    private void Awake()
    {
        if (_uiDocument == null)
            _uiDocument = GetComponent<UIDocument>();
    }

    // called once during start
    private void Start()
    {
        _viewModel = new UserViewModel();

        // get current user from SessionData
        var CurrentUser = SessionData.CurrentUser;
        Debug.Log("Herzlich Willkommen, " + CurrentUser.Name + "! Hier kannst du deine persönlichen Daten überarbeiten");

        // connects ui-events
        SetupUIEventHandlers();
    }

    // changes from UI --> update ViewModel (for login)
    private void SetupUIEventHandlers()
    {
        // connects input (variables) with functions --> function gets called every time, when sth changes in inputfield in UI
        if (NameInput != null)
        {
            NameInput.onValueChanged.AddListener(OnNameChanged);
        }
        if (UserNameInput != null)
        {
            UserNameInput.onValueChanged.AddListener(OnUsernameChanged);
        }
        if (MailInput != null)
        {
            MailInput.onValueChanged.AddListener(OnMailChanged);
        }
        if (PasswordInput != null)
        {
            PasswordInput.onValueChanged.AddListener(OnPasswordChanged);
        }
    }
    // functions: change variables in viewModel if sth typed in input (new value of textinput == var value in ViewModel)
    private void OnNameChanged(string value)
    {
        if (_viewModel != null)
        {
            _viewModel.Name = value;
        }
    }
    private void OnUsernameChanged(string value)
    {
        if (_viewModel != null)
        {
            _viewModel.Username = value;
        }
    }
    private void OnMailChanged(string value)
    {
        if (_viewModel != null)
        {
            _viewModel.Mail = value;
        }
    }
    private void OnPasswordChanged(string value)
    {
        if (_viewModel != null)
        {
            _viewModel.Password = value;
        }
    }

    // functionality of login button (UI)
    public void OnBackPressed()
    {
        // back to login
        SceneManager.LoadScene("LoginScene");
    }

    // functionality of account button
    public void OnUpdatePressed()
    {
        StartCoroutine(UpdateUser());
    }

    private IEnumerator UpdateUser()
    {
        Task loadTask = _viewModel.UpdateUserAsync();
        yield return new WaitUntil(() => loadTask.IsCompleted);
    }
}
