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
        }

        // Bewegung per Transform
        /*public void MoveAnimalRequested(GameObject instance)
        {
            if (instance != null)
            {
                var mb = instance.GetComponent<MonoBehaviourBridge>();
                if (mb != null)
                {
                    mb.StartCoroutine(MovementLibrary.MoveByTransform(instance, Vector3.left * 3f, 1.5f));
                }
            }
        }*/

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
