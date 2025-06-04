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
    public class RoadController : ControllerBase
    {
        /// Use connector to database!
        private readonly RoadRepository _repository = new();
        private readonly AnimalRoadRepository _repository2 = new();
        private readonly ElementRoadRepository _repository3= new();

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

        // Get related animals
        [HttpGet("allRelatedAnimals/{id}")]
        public IActionResult GetRelatedAnimals(string id)
        {
            var item = _repository2.GetByRoad(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // GetById
        [HttpGet("allRelatedElements/{id}")]
        public IActionResult GetRelatedElements(string id)
        {
            var item = _repository3.GetByRoad(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // Create
        [HttpPost("create")]
        public IActionResult Create([FromBody] backend.Models.Road newItem)
        {
            // id = + 1 --> prevent conflicts
            var allRoads = _repository.GetAll();
            int maxId = allRoads.Any() ? allRoads.Max(c => int.Parse(c.id)) : 0;
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
                var errorResponse = new { message = $"Strecke mit  ID {newItem.id} bereits vorhanden" };
                System.Diagnostics.Debug.WriteLine(errorResponse);
                return Conflict(errorResponse);
            }
        }


        // Update
        [HttpPut("update/{id}")]
        public IActionResult Update([FromBody] backend.Models.Road updatedItem)
        {
            var existingItem = _repository.GetById(updatedItem.id);
            if (existingItem == null)
            {
                return NotFound();
            }
            updatedItem.id = updatedItem.id;
            updatedItem.title = updatedItem.title;
            _repository.Update(updatedItem);
            return NoContent();
        }


        // Delete
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(string id)
        {
            // delete links to animals
            _repository2.DeleteByRoad(id);

            // delete links to element
            _repository3.DeleteByRoad(id);

            _repository.Delete(id);
            return NoContent();
        }
    }
}

