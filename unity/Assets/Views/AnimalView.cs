using UnityEngine;
using Assets.ViewModels;
using System.Collections;
using System.Threading.Tasks;

public class AnimalView : MonoBehaviour
{
    private AnimalViewModel _viewModel;
    private GameObject _animalInstance;

    private void Start()
    {
        _viewModel = new AnimalViewModel();
        StartCoroutine(LoadAndDisplayAnimals());
    }

    private IEnumerator LoadAndDisplayAnimals()
    {
        Task loadTask = _viewModel.LoadAllAnimalsAsync();
        yield return new WaitUntil(() => loadTask.IsCompleted);

        if (_viewModel.Animals == null || _viewModel.Animals.Count == 0)
        {
            Debug.LogWarning("Keine Tiere gefunden.");
            yield break;
        }

        var animal = _viewModel.Animals[2]; // Beispiel


        GameObject prefab = Resources.Load<GameObject>(animal.animationlink);
        if (prefab == null)
        {
            Debug.LogError($"Konnte Prefab nicht finden unter: Resources/{animal.animationlink}.prefab");
            yield break;
        }

        _animalInstance = Instantiate(prefab);

        Camera cam = Camera.main;
        _animalInstance.transform.position = cam.transform.position + cam.transform.forward * 3f + cam.transform.up * -1f; // etwas nach unten

        _animalInstance.AddComponent<MonoBehaviourBridge>();

        // ViewModel beauftragt die Bewegung (via Component oder direkt)
        yield return new WaitForSeconds(3f);
        //_viewModel.MoveAnimalRequested(_animalInstance);             // → transform
        _viewModel.MoveAnimalWithMoverRequested(_animalInstance);    // → CreatureMover
    }
}
