 //logic
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Assets.Models;
using Assets.Services;
using UnityEngine;
using Assets.Library;

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

            // select animal --> MANUALLY --> CHANGE LATER!!!
            if (Animals.Count > 0)
            {
                SelectedAnimal = Animals[6];
            }
        }

        // Move --> call to library (helpers). Library holds implementation of algorithms, etc.
        public void AnimalWalks(GameObject instance)
        {
            if (instance != null)
            {
                // get movement-component
                var mover = instance.AddComponent<CreatureMoverElk>();
                // bridge: enables coroutines (from viewmodel to view: view is unity specific --> no async-metods but coroutines)
                var mb = instance.GetComponent<MonoBehaviourBridge>();
                Animation anim = instance.GetComponent<Animation>();

                if (mover != null && mb != null)
                {
                    // move: run away
                    //mb.StartCoroutine(MovementLibrary.WalkNormal(mover));
                    mb.StartCoroutine(MovementLibraryElk.Walk(mover, anim));
                    // here second coroutine: will not work: coroutines dont bloxk and move over
                }
            }
        }
    }
}
