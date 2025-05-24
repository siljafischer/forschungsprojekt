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
    public class DiaryentryController : ControllerBase
    {
        /// Use connector to database!
        private readonly DiaryentryRepository _repository = new();
        private readonly DiaryDiaryentryRepository _repository2 = new();

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
        public IActionResult Create([FromBody] backend.Models.Diaryentry newItem)
        {
            // id = + 1 --> prevent conflicts
            var allEntries = _repository.GetAll();
            int maxId = allEntries.Any() ? allEntries.Max(c => int.Parse(c.id)) : 0;
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
                var errorResponse = new { message = $"Eintrag mit  ID {newItem.id} bereits vorhanden" };
                System.Diagnostics.Debug.WriteLine(errorResponse);
                return Conflict(errorResponse);
            }
        }

        // Create link to diary
        [HttpPost("createLinkToDiary")]
        public IActionResult CreateLinkToDiary([FromBody] backend.Models.DiaryDiaryentry newItem)
        {

            _repository2.Create(newItem);
            return NoContent();
        }


        // Update
        [HttpPut("update/{id}")]
        public IActionResult Update([FromBody] backend.Models.Diaryentry updatedItem)
        {
            var existingItem = _repository.GetById(updatedItem.id);
            if (existingItem == null)
            {
                return NotFound();
            }
            updatedItem.id = updatedItem.id;
            updatedItem.id_animal = updatedItem.id_animal;
            _repository.Update(updatedItem);
            return NoContent();
        }


        // Delete
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(string id)
        {
            // delete all links to diary
            _repository2.DeleteByDiaryentry(id);

            _repository.Delete(id);
            return NoContent();
        }

        // Delete by animal
        [HttpDelete("deleteByAnimal/{id}")]
        public IActionResult DeleteByAnimal(string id)
        {
            // delete all links to diary
            _repository2.DeleteByDiaryentry(id);

            _repository.DeleteByAnimal(id);
            return NoContent();
        }
    }
}

