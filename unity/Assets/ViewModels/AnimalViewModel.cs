 //logic
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Assets.Models;
using Assets.Services;
using UnityEngine;
using Assets.Library;
using System.Linq;

namespace Assets.ViewModels
{
    public class AnimalViewModel
    {
        // connection to model (--> services)
        private readonly AnimalService _animalService;
        // list of all animals: observable collection can identify changes automatically
        public ObservableCollection<Animal> Animals { get; private set; }
        // selected animal --> binded to UI
        public Animal SelectedAnimal { get; private set; }

        public AnimalViewModel()
        {
            _animalService = new AnimalService();
            Animals = new ObservableCollection<Animal>();
        }

        // load all animals --> C# async: load but dont block
        public async Task LoadAllAnimalsAsync()
        {
            Animals.Clear();
            // call service (service calls api)
            var animals = await _animalService.GetAnimalsAsync();
            foreach (var animal in animals)
            {
                Animals.Add(animal);
            }

            // select animal
            if (Animals.Count > 0)
            {
                // randomly between 6 and 7 (wapiti or bear)
                int randomIndex = Random.Range(6, 8);
                SelectedAnimal = Animals[randomIndex];
            }
        }

        // load all animals --> C# async: load but dont block
        public async Task<Animal> LoadAnimalByIdAsync(string id)
        {
            // call service (service calls api)
            var animals = await _animalService.GetAnimalByIdAsync(id);
            // return animal
            return animals.FirstOrDefault();
        }

        // Move --> call to library (helpers). Library holds implementation of algorithms, etc.
        public void AnimalWalks(GameObject instance)
        {
            if (instance != null)
            {
                // bridge: enables coroutines (from viewmodel to view: view is unity specific --> no async-metods but coroutines)
                var mb = instance.GetComponent<MonoBehaviourBridge>();

                // add animator ans controller for Animation
                /*
                 *  for new movement: add movement from .fbx to Controller
                 *  left: add newParam is<Movement>
                 *  create new state, add transitions to idle 
                 *  Edit Transition: add is<Movement>
                 *  --> copy everything else from walk
                 */

                // get animator of current animal --> also 20.000 cases bruachen wir ja eigentlich nicht, hier wäre Potenzial für Optimierungen
                // Wapiti/Elk
                if (SelectedAnimal.Id == "7")
                {
                    Animator animator = instance.AddComponent<Animator>();
                    animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("ElkController");
                    animator.applyRootMotion = false;
                    if (mb != null)
                    {
                        // movements
                        // movement unseen
                        mb.StartCoroutine(ElkMovementLibrary.MoveUnseen(animator, instance.transform, 3f, 2f));
                        // flee if person gets too close
                        // mb.StartCoroutine(ElkMovementLibrary.RunAway(animator, instance.transform, 3f, 25f));
                    }
                }
                // Bear
                if (SelectedAnimal.Id == "8")
                {
                    Animator animator = instance.GetComponent<Animator>();
                    animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("BearController");
                    animator.applyRootMotion = false;
                    if (mb != null)
                    {
                        // movements
                        // movement unseen
                        mb.StartCoroutine(BearMovementLibrary.MoveUnseen(animator, instance.transform, 3f, 10f));
                    }

                }

            }
        }
    }
}
