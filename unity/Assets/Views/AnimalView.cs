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
        // Tiere asynchron laden
        Task loadTask = _viewModel.LoadAllAnimalsAsync();
        yield return new WaitUntil(() => loadTask.IsCompleted);

        if (_viewModel.Animals == null || _viewModel.Animals.Count == 0)
        {
            Debug.LogWarning("Keine Tiere gefunden.");
            yield break;
        }

        // Erstes Tier holen
        var animal = _viewModel.Animals[0];
        string path = animal.animationlink;

        Debug.Log($"Lade GLB-Modell von Pfad: {path}");

        // GLB-Datei laden
        var gltf = new GltfImport();
        Task<bool> loadModel = gltf.Load(path);
        yield return new WaitUntil(() => loadModel.IsCompleted);

        if (loadModel.Result)
        {
            GameObject go = new GameObject(animal.name);
            gltf.InstantiateMainScene(go.transform);

            // Optional: Positionieren
            go.transform.position = new Vector3(0, 0, 0);
            go.transform.localScale = Vector3.one;

            Debug.Log("Modell erfolgreich geladen und instanziiert.");
        }
        else
        {
            Debug.LogError("Fehler beim Laden des GLB-Modells.");
        }
    }
}
