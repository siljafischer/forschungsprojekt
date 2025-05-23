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
    public class DiaryController : ControllerBase
    {
        /// Use connector to database!
        private readonly DiaryRepository _repository = new();
        private readonly DiaryDiaryentryRepository _repository2 = new();


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

        // GetByUser
        [HttpGet("allByUser/{user}")]
        public IActionResult GetByUser(string user)
        {
            var item = _repository.GetByUser(user);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // GetRelatedEntries
        [HttpGet("allRelatedEntries/{id}")]
        public IActionResult GetRelatedEntries(string id)
        {
            var item = _repository2.GetByDiary(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // Create
        [HttpPost("create")]
        public IActionResult Create([FromBody] backend.Models.Diary newItem)
        {
            // id = + 1 --> prevent conflicts
            var allDiaries = _repository.GetAll();
            int maxId = allDiaries.Any() ? allDiaries.Max(c => int.Parse(c.id)) : 0;
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
                var errorResponse = new { message = $"Tagebuch mit  ID {newItem.id} bereits vorhanden" };
                System.Diagnostics.Debug.WriteLine(errorResponse);
                return Conflict(errorResponse);
            }
        }


        // Update
        [HttpPut("update/{id}")]
        public IActionResult Update([FromBody] backend.Models.Diary updatedItem)
        {
            var existingItem = _repository.GetById(updatedItem.id);
            if (existingItem == null)
            {
                return NotFound();
            }
            updatedItem.id = updatedItem.id;
            updatedItem.user = updatedItem.user;
            _repository.Update(updatedItem);
            return NoContent();
        }


        // Delete
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(string id)
        {
            // delete related entries
            _repository2.DeleteByDiary(id);

            _repository.Delete(id);
            return NoContent();
        }
    }
}

