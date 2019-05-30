using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Entities;
using TodoList.Api.Repositories;

namespace TodoList.Api.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        public TodoItem GetItem(Guid listId, Guid id) { return _repository.GetItem(listId, id); }

        public IEnumerable<TodoItem> GetAllItemsofList(Guid listId) { return _repository.GetAllItemsofList(listId); }

        public TodoItem CreateItem(Guid listId, TodoItem item) { return _repository.SaveItem(listId, item); }

        public TodoItem UpdateItem(Guid listId, TodoItem item) { return _repository.UpdateItem(listId, item); }

        public bool DeleteItem(Guid listId, Guid id) { return _repository.MarkItemAsDeleted(listId, id); }

        public TodoItemList GetList(Guid id) { return _repository.GetList(id); }

        public IEnumerable<TodoItemList> GetAllLists() { return _repository.GetAllLists(); }

        public TodoItemList CreateList(TodoItemList list) { return _repository.SaveList(list); }

        public TodoItemList UpdateList(TodoItemList list) { return _repository.UpdateList(list); }

        public bool DeleteList(Guid id) {
            if (_repository.GetAllItemsofList(id).ToList().Count() > 0)
                return false;
            return _repository.MarkListAsDeleted(id);
        }

        public TodoItemListWithItems GetListWithItems(Guid id)
        {
            var list = _repository.GetList(id);
            var items = _repository.GetAllItemsofList(id);

            if (list == null)
                return null;

            return new TodoItemListWithItems {
                Id = list.Id,
                Name = list.Name,
                Items = items.ToList()
            };
        }
    }

    public interface ITodoService
    {
        TodoItem GetItem(Guid id, Guid id1);
        IEnumerable<TodoItem> GetAllItemsofList(Guid listId);
        TodoItem CreateItem(Guid listId, TodoItem item);
        TodoItem UpdateItem(Guid listId, TodoItem item);
        bool DeleteItem(Guid id, Guid id1);


        TodoItemList GetList(Guid id);
        IEnumerable<TodoItemList> GetAllLists();
        TodoItemList CreateList(TodoItemList list);
        TodoItemList UpdateList(TodoItemList list);
        bool DeleteList(Guid id);

        TodoItemListWithItems GetListWithItems(Guid id);
    }
}
