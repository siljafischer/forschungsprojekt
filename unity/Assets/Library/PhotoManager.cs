using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.InputSystem;
using Assets.Views;
using Assets.Models;

public class PhotoManager : MonoBehaviour
{
    [Header("UI & Controller")]
    public GameObject[] uiElements;        // UI to hide
    public GameObject[] controllerModels;  // controller to hide

    [Header("Animal View")]
    public AnimalView animalView;

    [System.Obsolete]
    private void Start()
    {
        if (animalView == null)
        {
            animalView = FindObjectOfType<AnimalView>();
            if (animalView == null)
                Debug.LogWarning("AnimalView nicht gefunden.");
        }
    }

    private void Update()
    {
        // Laptop: space key to capture photo
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryCapturePhoto();
        }

        // Controller: "A" button or index trigger to capture photo:
    }

    private void TryCapturePhoto()
    {
        if (animalView != null && animalView.IsViewfinderActive())
        {
            StartCoroutine(CapturePhoto());
        }
        else
        {
            Debug.Log("Viewfinder ist nicht aktiv. Kein Foto möglich.");
        }
    }

    private IEnumerator CapturePhoto()
    {
        // hide UI and controllers
        foreach (var ui in uiElements)
            if (ui != null) ui.SetActive(false);

        foreach (var ctrl in controllerModels)
            if (ctrl != null) ctrl.SetActive(false);

        yield return new WaitForEndOfFrame();

        string folderPath = Path.Combine(Application.dataPath, "Resources/Photo");
        Directory.CreateDirectory(folderPath);

        // get active animal name
        string animalName = "Unknown";
        var selectedAnimal = animalView.GetSelectedAnimal();
        if (selectedAnimal != null)
        {
            string name = selectedAnimal.Name;
            if (!string.IsNullOrEmpty(name))
            {
                animalName = char.ToLower(name[0]) + name.Substring(1);
            }
        }

        string filename = Path.Combine(folderPath, $"{animalName}_unity.png");

        // take photo
        ScreenCapture.CaptureScreenshot(filename);
        Debug.Log($"Screenshot gespeichert: {filename}");

        // activate UI and controllers
        foreach (var ui in uiElements)
            if (ui != null) ui.SetActive(true);

        foreach (var ctrl in controllerModels)
            if (ctrl != null) ctrl.SetActive(true);
    }
}
