// Communication: Definition of API-Endpoints
using System.Diagnostics;
using System.Globalization;
using backend.Controllers;
// use classes, mapper, DoorController
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnimalController : ControllerBase
    {
        /// Use connector to calnedar-database!
        private readonly AnimalRepository _repository = new();
        private readonly AnimalRoadRepository _repository2 = new();

        // Get all
        [HttpGet]
        public IActionResult GetAll()
        {
            var items = _repository.GetAll();
            return Ok(items);
        }


        // GetById
        [HttpGet("allById/{id}")]
        public IActionResult GetById(string id)
        {
            var item = _repository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // Create
        [HttpPost("create")]
        public IActionResult Create([FromBody] backend.Models.Animal newItem)
        {
            // id = + 1 --> prevent conflicts
            var allAnimals = _repository.GetAll();
            int maxId = allAnimals.Any() ? allAnimals.Max(c => int.Parse(c.id)) : 0;
            newItem.id = (maxId + 1).ToString();

            // only create, if id not in database (double-check)
            var existItem = _repository.GetById(newItem.id);
            if (existItem == null)
            {
                _repository.Create(newItem);
                return NoContent();
            }
            else
            {
                var errorResponse = new { message = $"Tier mit  ID {newItem.id} bereits vorhanden" };
                System.Diagnostics.Debug.WriteLine(errorResponse);
                return Conflict(errorResponse);
            }
        }

        // Create Link To Road
        [HttpPost("createLinkToRoad")]
        public IActionResult CreateLinkToRoad([FromBody] backend.Models.AnimalRoad newItem) { 

            _repository2.Create(newItem);
            return NoContent();

        }


        // Update
        [HttpPut("update/{id}")]
        public IActionResult Update([FromBody] backend.Models.Animal updatedItem)
        {
            var existingItem = _repository.GetById(updatedItem.id);
            if (existingItem == null)
            {
                return NotFound();
            }
            updatedItem.id = updatedItem.id;
            updatedItem.name = updatedItem.name;
            updatedItem.animationlink = updatedItem.animationlink;
            updatedItem.habitat = updatedItem.habitat;
            _repository.Update(updatedItem);
            return NoContent();
        }


        // Delete
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(string id)
        {
            // delete all links to roads
            _repository2.DeleteByAnimal(id);

            _repository.Delete(id);
            return NoContent();
        }
    }
}

