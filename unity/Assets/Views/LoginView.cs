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
public class LoginView : MonoBehaviour
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
    public TMP_InputField UserNameInput;
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

        // connects ui-events
        SetupUIEventHandlers();
    }

    // changes from UI --> update ViewModel (for login)
    private void SetupUIEventHandlers()
    {
        // connects input (variables) with functions --> function gets called every time, when sth changes in inputfield in UI
        if (UserNameInput != null)
        {
            UserNameInput.onValueChanged.AddListener(OnUsernameChanged);
        }
        if (PasswordInput != null)
        {
            PasswordInput.onValueChanged.AddListener(OnPasswordChanged);
        }
    }
    // functions: change variables in viewModel if sth typed in input (new value of textinput == var value in ViewModel)
    private void OnUsernameChanged(string value)
    {
        if (_viewModel != null)
        {
            _viewModel.LoginUsername = value;
        }
    }
    private void OnPasswordChanged(string value)
    {
        if (_viewModel != null)
        {
            _viewModel.LoginPassword = value;
        }
    }

    // functionality of button (UI)
    public void OnLoginPressed()
    {
        // login for user
        StartCoroutine(Login());
    }

    // login for user
    private IEnumerator Login()
    {
        // load user
        Task loadTask = _viewModel.LoadUserByUsernameAsync();
        // WaitUntil ~ async/await: wait until task ist completed)
        yield return new WaitUntil(() => loadTask.IsCompleted);

        // only reached if login is successfull (logic in view model)
        Debug.Log($"Login erfolgreich für User: {_viewModel.SelectedUser.name}");
        // change scene
        SceneManager.LoadScene("first_steps");
    }
}
