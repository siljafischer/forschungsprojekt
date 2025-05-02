using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Assets.Models;
using Assets.Services;

namespace Assets.ViewModels
{
    public class AnimalViewModel
    {
        private readonly AnimalService _animalService;

        // ObservableCollection
        public ObservableCollection<Animal> Animals { get; private set; }

        public AnimalViewModel()
        {
            _animalService = new AnimalService();
            Animals = new ObservableCollection<Animal>();
        }

        // Zeige alle Tiere (hier eine asynchrone Methode)
        public async Task LoadAllAnimalsAsync()
        {
            Animals.Clear();
            var animals = await _animalService.GetAnimalsAsync();
            foreach (var animal in animals)
            {
                Animals.Add(animal);
            }
        }
    }
}
