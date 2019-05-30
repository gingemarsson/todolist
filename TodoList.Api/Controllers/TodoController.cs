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
    [Route("api/list")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;
        public TodoController(ITodoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all information about the specified todo list.
        /// </summary>
        /// <param name="listId">Id of the specified list.</param>
        /// <returns></returns>
        [HttpGet("{listId}")]
        public ActionResult<TodoItemListWithItems> GetList([FromRoute] Guid listId)
        {
            var res = _service.GetListWithItems(listId);

            if (res == null)
                return NotFound();

            return res;
        }

        /// <summary>
        /// Get a specified item from a todo list.
        /// </summary>
        /// <param name="listId">Id of the specified list.</param>
        /// <param name="Id">Id of the specified item.</param>
        /// <returns></returns>
        [HttpGet("{listId}/items/{id}")]
        public ActionResult<TodoItem> GetItem([FromRoute] Guid listId, [FromRoute] Guid Id)
        {
            var res = _service.GetItem(listId, Id);

            if (res == null)
                return NotFound();

            return res;
        }

        /// <summary>
        /// Create a new item at specified todo list.
        /// </summary>
        /// <param name="listId">Id of the specified list.</param>
        /// <param name="request">The item to create.</param>
        /// <returns></returns>
        [HttpPost("{listId}")]
        public ActionResult<TodoItem> CreateItem([FromRoute] Guid listId, [FromBody] TodoItemRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var todoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Done = request.Done
            };
                      

            return _service.CreateItem(listId, todoItem);

        }

        /// <summary>
        /// Update the specified item at the specified todo list.
        /// </summary>
        /// <param name="listId">Id of the item's list.</param>
        /// <param name="Id">Id of the item to update.</param>
        /// <param name="request">The updated item.</param>
        /// <returns></returns>
        [HttpPut("{listId}/items/{id}")]
        public ActionResult<TodoItem> UpdateItem([FromRoute] Guid listId, [FromRoute] Guid Id, [FromBody] TodoItemRequest request)
        {
            var todoItem = new TodoItem
            {
                Id = Id,
                Name = request.Name,
                Description = request.Description,
                Done = request.Done
            };

            var res = _service.UpdateItem(listId, todoItem);

            if (res == null)
                return NotFound();

            return res;
        }

        /// <summary>
        /// Delete the specified item at the specified todo list.
        /// </summary>
        /// <param name="listId">Id of the specified list.</param>
        /// <param name="Id">Id of the specified item.</param>
        /// <returns></returns>
        [HttpDelete("{listId}/items/{id}")]
        public ActionResult DeleteItem([FromRoute] Guid listId, [FromRoute] Guid Id)
        {
            var res = _service.DeleteItem(listId, Id);
            return res ? Ok() : StatusCode(500);
        }

        //Lists

        /// <summary>
        /// Get a list of todo-lists
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<TodoItemList>> GetAllLists()
        {
            return _service.GetAllLists().ToList();
        }

        /// <summary>
        /// Create a new todo list.
        /// </summary>
        /// <param name="request">The todo list to create.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<TodoItemList> CreateList([FromBody] TodoListRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var todoItemList = new TodoItemList
            {
                Id = Guid.NewGuid(),
                Name = request.Name,

            };


            return _service.CreateList(todoItemList);

        }

        /// <summary>
        /// Update the specified todo list.
        /// </summary>
        /// <param name="listId">Id of the specified list.</param>
        /// <param name="request">The updated todo list.</param>
        /// <returns></returns>
        [HttpPut("{listId}")]
        public ActionResult<TodoItemList> UpdateList([FromRoute] Guid listId, [FromBody] TodoListRequest request)
        {
            var todoItemList = new TodoItemList
            {
                Id = listId,
                Name = request.Name
            };

            var res = _service.UpdateList(todoItemList);

            if (res == null)
                return NotFound();

            return res;
        }

        /// <summary>
        /// Delete the specified todo list. The specified list must be empty.
        /// </summary>
        /// <param name="listId">Id of the todo list to delete.</param>
        /// <returns></returns>
        [HttpDelete("{listId}")]
        public ActionResult DeleteList([FromRoute] Guid listId)
        {
            var res = _service.DeleteList(listId);
            return res ? Ok() : StatusCode(400);
        }
    }
}