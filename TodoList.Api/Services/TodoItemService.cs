using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Entities;
using TodoList.Api.Repositories;

namespace TodoList.Api.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _repository;

        public TodoItemService(ITodoItemRepository repository)
        {
            _repository = repository;
        }

        public TodoItem Get(Guid id) { return _repository.Get(id); }

        public IEnumerable<TodoItem> GetAll() { return _repository.GetAll(); }

        public TodoItem Create(TodoItem item) { return _repository.Save(item); }

        public TodoItem Update(TodoItem item) { return _repository.Update(item); }

        public bool Delete(Guid id) { return _repository.MarkAsDeleted(id); }
    }

    public interface ITodoItemService
    {
        TodoItem Get(Guid id);
        IEnumerable<TodoItem> GetAll();
        TodoItem Create(TodoItem item);
        TodoItem Update(TodoItem item);
        bool Delete(Guid id);
    }
}
