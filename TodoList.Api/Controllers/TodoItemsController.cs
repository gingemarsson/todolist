using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoList.Api.Entities;
using TodoList.Api.Requests;
using TodoList.Api.Services;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemService _service;
        public TodoItemsController(ITodoItemService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get the whole list of items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> Get()
        {
            return _service.GetAll().ToList();
        }

        // Get the item with specified Id
        [HttpGet("{id}")]
        public ActionResult<TodoItem> Get([FromRoute] Guid Id)
        {
            var res = _service.Get(Id);

            if (res == null)
                return NotFound();

            return res;
        }

        // Create new item the item with specified ID
        [HttpPost]
        public ActionResult<TodoItem> Post([FromBody] TodoItemRequest request)
        {
            var todoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Done = request.Done
            };

            return _service.Create(todoItem);
        }

        // Update the item with specified ID
        [HttpPut("{id}")]
        public ActionResult<TodoItem> Put([FromRoute] Guid Id, [FromBody] TodoItemRequest request)
        {
            var todoItem = new TodoItem
            {
                Id = Id,
                Name = request.Name,
                Description = request.Description,
                Done = request.Done
            };

            var res = _service.Update(todoItem);

            if (res == null)
                return NotFound();

            return res;
        }

        // Delete the item with specified ID
        [HttpDelete("{id}")]
        public ActionResult<TodoItem> Delete([FromRoute] Guid Id)
        {
            var res = _service.Delete(Id);
            return res ? Ok() : StatusCode(500);
        }
    }
}