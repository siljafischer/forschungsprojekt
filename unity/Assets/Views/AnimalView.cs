using UnityEngine;
using Assets.ViewModels;
using System.Collections;
using System.Threading.Tasks;

public class AnimalView : MonoBehaviour
{
    private AnimalViewModel _viewModel;

    private void Start()
    {
        _viewModel = new AnimalViewModel();
        StartCoroutine(LoadAndDisplayAnimals()); // Starte Coroutine für asynchrone Methode
    }

    // Coroutine, um asynchrone Methode korrekt auszuführen
    private IEnumerator LoadAndDisplayAnimals()
    {
        // Warten auf den Abschluss des asynchronen Tasks
        Task task = _viewModel.LoadAllAnimalsAsync();
        yield return new WaitUntil(() => task.IsCompleted); // Warten, bis der Task abgeschlossen ist

        // Alle Tiere in der Konsole ausgeben
        foreach (var animal in _viewModel.Animals)
        {
            Debug.Log($"Tier: {animal.name}");
        }
    }
}
