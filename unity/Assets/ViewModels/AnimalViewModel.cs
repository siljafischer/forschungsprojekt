using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Assets.Models;
using Assets.Services;
using UnityEngine;
using Assets.Library;
using Controller;

namespace Assets.ViewModels
{
    public class AnimalViewModel
    {
        private readonly AnimalService _animalService;
        public ObservableCollection<Animal> Animals { get; private set; }

        // aktuell ausgewähltes Tier, das ans UI gebunden wird
        public Animal SelectedAnimal { get; private set; }

        public AnimalViewModel()
        {
            _animalService = new AnimalService();
            Animals = new ObservableCollection<Animal>();
        }

        public async Task LoadAllAnimalsAsync()
        {
            Animals.Clear();
            var animals = await _animalService.GetAnimalsAsync();
            foreach (var animal in animals)
            {
                Animals.Add(animal);
            }

            if (Animals.Count > 0)
            {
                SelectedAnimal = Animals[2]; // Beispiel: Erstes Tier auswählen
            }
        }

        // Bewegung per CreatureMover
        public void MoveAnimalWithMoverRequested(GameObject instance)
        {
            if (instance != null)
            {
                var mover = instance.GetComponent<CreatureMovement>();
                var mb = instance.GetComponent<MonoBehaviourBridge>();

                if (mover != null && mb != null)
                {
                    mb.StartCoroutine(MovementLibrary.MoveWithCreatureMover(mover, speed: 0.1f, duration: 2f));
                }
            }
        }
    }
}
