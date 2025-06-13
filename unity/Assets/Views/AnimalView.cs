using UnityEngine;
using Assets.ViewModels;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using Assets.Models;
using UnityEngine.SceneManagement;

// gets UIDocument-Component 
[RequireComponent(typeof(UIDocument))]
public class AnimalView : MonoBehaviour
{
    // connection to viewmodel (View Model contains Logic)
    private AnimalViewModel _viewModel;
    // stores current selected animal (only one for databinding)
    private GameObject _animalInstance;
    // root of UI --> acceess UI (e.g. change text, use buttons)
    private VisualElement _root;

    // automatic call: activate object (e.g. gamestart)
    private void Start()
    {
        _viewModel = new AnimalViewModel();

        // get current user from SessionData
        var CurrentUser = SessionData.CurrentUser;
        Debug.Log("Herzlich Willkommen, " + CurrentUser.Name + "! Los geht's, fotografiere dein erstes Tier!");

        // load animals async (Coroutine ~ async/await: wait but dont block game)
        StartCoroutine(LoadAndDisplayAnimals());
    }

    // load and show animal
    private IEnumerator LoadAndDisplayAnimals()
    {
        // load animals and wait until animals are loaded
        Task loadTask = _viewModel.LoadAllAnimalsAsync();
        // called by Coroutine --> WaitUntil ~ async/await: wait until task ist completed)
        yield return new WaitUntil(() => loadTask.IsCompleted);

        if (_viewModel.Animals == null || _viewModel.Animals.Count == 0)
        {
            Debug.LogWarning("Keine Tiere gefunden.");
            yield break;
        }

        // get selected animal from view model (--> initiated load and now get the selected animal)
        var selectedAnimal = _viewModel.SelectedAnimal;


        // DataBinding
        // connect and get root-element of uxml (UI-Root)
        var visualTree = Resources.Load<VisualTreeAsset>("AnimalList");
        _root = visualTree.CloneTree();
        if (_root == null)
        {
            Debug.LogError("Kein Root VisualElement gefunden.");
            yield break;
        }
        GetComponent<UIDocument>().rootVisualElement.Add(_root);
        // dataSource connects UI <-> selectedAnimal
        _root.dataSource = selectedAnimal;


        // load and instantiate prefab (3D-Object) from "animationlink" (Resources/name.prefab)
        GameObject prefab = Resources.Load<GameObject>(selectedAnimal.animationlink);
        if (prefab == null)
        {
            Debug.LogError($"Konnte Prefab nicht finden unter: Resources/{selectedAnimal.animationlink}");
            yield break;
        }
        _animalInstance = Instantiate(prefab);


        // Position: 3 meters in front of camera
        Camera cam = Camera.main;
        _animalInstance.transform.position = cam.transform.position + cam.transform.forward * 3f + cam.transform.up * -1f;

        // Script of animal
        _animalInstance.AddComponent<MonoBehaviourBridge>();

        // wait 3 seconds and move --> BEHAVIOUR INTO VIEWMODEL!!!
        yield return new WaitForSeconds(2f);
        _viewModel.AnimalWalks(_animalInstance);
    }

    // back button
    public void OnBackPressed()
    {
        // back to login
        SceneManager.LoadScene("MenuScene");
    }
}
