// IN DIESER DATEI LOGINFORMULAR VERLINKEN --> BUTTONS VMTL IN RESOURCES/USERLIST.UXML EINBINDEN
using UnityEngine;
using Assets.ViewModels;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using Assets.Models;
//using Oculus.Platform;

// gets UIDocument-Component 
[RequireComponent(typeof(UIDocument))]
public class UserView : MonoBehaviour
{
    // connection to viewmodel (View Model contains Logic)
    private UserViewModel _viewModel;
    // stores current selected user (only one for databinding)
    private GameObject _userInstance;
    // root of UI --> acceess UI (e.g. change text, use buttons)
    [SerializeField] private UIDocument _uiDocument;
    private VisualElement _root;

    // automatic call: activate object (e.g. gamestart)
    private void Start()
    {
        _viewModel = new UserViewModel();
        // LOAD BY START --> SOLLTE FÜR LOGIN GEÄNDERT/ENTFERNT WERDEN
        // load users async (Coroutine ~ async/await: wait but dont block game)
        StartCoroutine(LoadAndDisplayUser());
    }

    // load and show user
    private IEnumerator LoadAndDisplayUser()
    {
        // load user
        Task loadTask = _viewModel.LoadUserByUsernameAsync();
        // called by Coroutine --> WaitUntil ~ async/await: wait until task ist completed)
        yield return new WaitUntil(() => loadTask.IsCompleted);

        if (_viewModel.Users == null || _viewModel.Users.Count == 0)
        {
            Debug.LogWarning("Keinen User gefunden.");
            yield break;
        }

        // get selected user from view model (--> initiated load and now get the selected user)
        var selectedUser = _viewModel.SelectedUser;


        // DataBinding
        // connect and get root-element of uxml (UI-Root)
        var visualTree = Resources.Load<VisualTreeAsset>("UserList");
        _root = visualTree.CloneTree();
        if (_root == null)
        {
            Debug.LogError("Kein Root VisualElement gefunden.");
            yield break;
        }
        GetComponent<UIDocument>().rootVisualElement.Add(_root);
        // dataSource connects UI <-> selectedUser
        _root.dataSource = selectedUser;


        // show user id in console --> ONLY FOR TESTING
        if (selectedUser != null)
        {
            Debug.LogFormat($"Folgenden User gefunden {selectedUser.name}: {selectedUser.username}");
            yield break;
        }
    }
}
