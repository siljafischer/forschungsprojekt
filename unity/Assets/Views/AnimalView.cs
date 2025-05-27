using UnityEngine;
using Assets.ViewModels;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using Assets.Models;

[RequireComponent(typeof(UIDocument))]
public class AnimalView : MonoBehaviour
{
    private AnimalViewModel _viewModel;
    private GameObject _animalInstance;
    private VisualElement _root;

    private void Start()
    {
        _viewModel = new AnimalViewModel();
        StartCoroutine(LoadAndDisplayAnimals());
    }

    private IEnumerator LoadAndDisplayAnimals()
    {
        // Tiere laden über ViewModel
        Task loadTask = _viewModel.LoadAllAnimalsAsync();
        yield return new WaitUntil(() => loadTask.IsCompleted);

        if (_viewModel.Animals == null || _viewModel.Animals.Count == 0)
        {
            Debug.LogWarning("Keine Tiere gefunden.");
            yield break;
        }

        // Zugriff auf das ausgewählte Tier
        var selectedAnimal = _viewModel.SelectedAnimal;

        // UIDocument laden und an UI Toolkit anbinden
        var uiDocument = GetComponent<UIDocument>();
        _root = uiDocument.rootVisualElement;

        if (_root == null)
        {
            Debug.LogError("Kein Root VisualElement gefunden.");
            yield break;
        }

        // DataBinding mit dem ausgewählten Tier
        _root.dataSource = selectedAnimal;

        // Tier-Prefab laden und instanziieren
        GameObject prefab = Resources.Load<GameObject>(selectedAnimal.animationlink);
        if (prefab == null)
        {
            Debug.LogError($"Konnte Prefab nicht finden unter: Resources/{selectedAnimal.animationlink}.prefab");
            yield break;
        }

        _animalInstance = Instantiate(prefab);

        // Tier positionieren
        Camera cam = Camera.main;
        _animalInstance.transform.position = cam.transform.position + cam.transform.forward * 3f + cam.transform.up * -1f;

        _animalInstance.AddComponent<MonoBehaviourBridge>();

        // Bewegung nach kurzer Pause starten
        yield return new WaitForSeconds(3f);
        _viewModel.MoveAnimalWithMoverRequested(_animalInstance);
    }
}
