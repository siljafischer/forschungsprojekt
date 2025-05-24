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
    public class UserController : ControllerBase
    {
        /// Use connector to database!
        private readonly UserRepository _repository = new();
        private readonly DiaryRepository _repository2 = new();

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

        // GetByUsername
        [HttpGet("allByUsername/{username}")]
        public IActionResult GetByUsername(string username)
        {
            var item = _repository.GetByUsername(username);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // Create
        [HttpPost("create")]
        public IActionResult Create([FromBody] backend.Models.User newItem)
        {
            // id = + 1 --> prevent conflicts
            var allUsers = _repository.GetAll();
            int maxId = allUsers.Any() ? allUsers.Max(c => int.Parse(c.id)) : 0;
            newItem.id = (maxId + 1).ToString();

            // only create, if id AND username not in database (double-check)
            var existItem = _repository.GetById(newItem.id);
            var existItem2 = _repository.GetByUsername(newItem.username);
            if (existItem == null && existItem2 == null)
            {
                // Create related diary
                var newDiary = new backend.Models.Diary
                {
                    id = "0",
                    user = $"{newItem.id}"
                };
                _repository2.Create(newDiary);

                _repository.Create(newItem);
                return NoContent();
            }
            else
            {
                var errorResponse = new { message = $"Nutzer mit  ID {newItem.id} oder Name {newItem.username} bereits vorhanden" };
                System.Diagnostics.Debug.WriteLine(errorResponse);
                return Conflict(errorResponse);
            }
        }


        // Update
        [HttpPut("update/{id}")]
        public IActionResult Update([FromBody] backend.Models.User updatedItem)
        {
            var existingItem = _repository.GetById(updatedItem.id);
            if (existingItem == null)
            {
                return NotFound();
            }
            updatedItem.id = updatedItem.id;
            updatedItem.name = updatedItem.name;
            updatedItem.username = updatedItem.username;
            updatedItem.mail = updatedItem.mail;
            updatedItem.password = updatedItem.password;
            _repository.Update(updatedItem);
            return NoContent();
        }


        // Delete
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(string id)
        {
            // Delete diary
            _repository2.DeleteByUser(id);

            _repository.Delete(id);
            return NoContent();
        }
    }
}

