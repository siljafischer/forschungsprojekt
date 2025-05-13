using UnityEngine;
using Assets.ViewModels;
using System.Collections;
using System.Threading.Tasks;
using GLTFast;

public class AnimalView : MonoBehaviour
{
    private AnimalViewModel _viewModel;

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

        Debug.Log($"Lade Tier: {animal.name}, Pfad: {animal.animationlink}");

        // Dynamisches Prefab-Laden
        GameObject prefab = Resources.Load<GameObject>(animal.animationlink);
        if (prefab == null)
        {
            Debug.LogError($"Konnte Prefab nicht finden unter: Resources/{animal.animationlink}.prefab");
            yield break;
        }

        GameObject instance = Instantiate(prefab);

        // Positionieren in Sichtweite
        Camera cam = Camera.main;
        instance.transform.position = cam.transform.position + cam.transform.forward * 3f;

        Debug.Log("Tier erfolgreich geladen und angezeigt.");
    }

}
