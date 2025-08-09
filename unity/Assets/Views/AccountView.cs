using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.ViewModels;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using Assets.Models;
using TMPro;
using Unity.Properties;

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
    // var for current user
    public User CurrentUser;
    public string BackupOldPassword;

    // TextInputs
    [Header("UI Input Fields")]
    public TMP_InputField NameInput;
    public TMP_InputField UserNameInput;
    public TMP_InputField MailInput;
    public TMP_InputField PasswordOld;
    public TMP_InputField PasswordNewInput;

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
        CurrentUser = SessionData.CurrentUser;
        Debug.Log("Herzlich Willkommen, " + CurrentUser.Name + "! Hier kannst du deine persönlichen Daten überarbeiten");

        // connects ui-events
        SetupUIEventHandlers();
        // connects ui-viewmodel (gets changes from ViewModel)
        SetupDataBinding();

        // Show user data in InputFields
        if (NameInput != null)
            NameInput.text = CurrentUser.Name;
        if (UserNameInput != null)
            UserNameInput.text = CurrentUser.Username;
        if (MailInput != null)
            MailInput.text = CurrentUser.Mail;
        if (PasswordOld != null)
            BackupOldPassword = CurrentUser.Password;
            PasswordOld.text = "Passwort durch Eingabe ändern: " + CurrentUser.Password;
    }

    /*
     * UPDATE VIEWMODEL
    */
    // changes from UI --> update ViewModel (for login)
    private void SetupUIEventHandlers()
    {
        // connects input (variables) with functions --> function gets called every time, when sth changes in inputfield in UI
        if (NameInput != null)
        {
            NameInput.onValueChanged.AddListener((value) => _viewModel.UserName = value);
        }
        if (UserNameInput != null)
        {
            UserNameInput.onValueChanged.AddListener((value) => _viewModel.UserUsername = value);
        }
        if (MailInput != null)
        {
            MailInput.onValueChanged.AddListener((value) => _viewModel.UserMail = value);
        }
        if (PasswordNewInput != null)
        {
            PasswordNewInput.onValueChanged.AddListener((value) => _viewModel.UserPassword = value);
        }
    }


    /*
     * UPDATE VIEW (from ViewModel)
    */
    private void SetupDataBinding()
    {
        if (_viewModel == null) return;

        // get updates from viewmodel
        UpdateUIFromViewModel();
        _viewModel.propertyChanged += OnViewModelPropertyChanged;
    }
    // get changes from view model --> update UI
    private void OnViewModelPropertyChanged(object sender, BindablePropertyChangedEventArgs e)
    {
        // update UI --> change, if change in viewmodel
        switch (e.propertyName)
        {
            case nameof(UserViewModel.UserName):
                if (NameInput != null && NameInput.text != _viewModel.UserName)
                {
                    NameInput.text = _viewModel.UserName;
                }
                break;
            case nameof(UserViewModel.UserUsername):
                if (UserNameInput != null && UserNameInput.text != _viewModel.UserUsername)
                {
                    UserNameInput.text = _viewModel.UserUsername;
                }
                break;
            case nameof(UserViewModel.UserMail):
                if (MailInput != null && MailInput.text != _viewModel.UserMail)
                {
                    MailInput.text = _viewModel.UserMail;
                }
                break;
            case nameof(UserViewModel.UserPassword):
                if (PasswordOld != null && PasswordOld.text != _viewModel.UserPassword)
                {
                    PasswordOld.text = "Änderung: " + _viewModel.UserPassword;
                }
                break;
        }
    }
    private void UpdateUIFromViewModel()
    {
        if (NameInput != null)
            NameInput.text = _viewModel.UserName;
        if (UserNameInput != null)
            UserNameInput.text = _viewModel.UserUsername;
        if (PasswordOld != null)
            PasswordOld.text = _viewModel.UserPassword;
        if (MailInput != null)
            MailInput.text = _viewModel.UserMail;
    }


    /*
     * FUNCTIONALITY OF BUTTONS
    */
    // functionality of back button
    public void OnBackPressed()
    {
        // for unsaved display: save "old" password
        _viewModel.UserPassword = BackupOldPassword;
        // back to menu
        SceneManager.LoadScene("MenuScene");
    }

    // functionality of update user
    public void OnUpdatePressed()
    {
        StartCoroutine(UpdateUser());
        // set new saved (!) password as backup password
        BackupOldPassword = _viewModel.UserPassword;
    }

    // functionality of delet user
    public void OnDeletePressed()
    {
        StartCoroutine(DeleteUser());
        SceneManager.LoadScene("LoginScene");
    }

    private IEnumerator UpdateUser()
    {
        Task loadTask = _viewModel.UpdateUserAsync();
        yield return new WaitUntil(() => loadTask.IsCompleted);
    }

    private IEnumerator DeleteUser()
    {
        Task loadTask = _viewModel.DeleteUserAsync();
        yield return new WaitUntil(() => loadTask.IsCompleted);
    }
}
