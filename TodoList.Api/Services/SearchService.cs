using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Entities;
using TodoList.Api.Repositories;

namespace TodoList.Api.Services
{
    public class SearchService : ISearchService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IUserRepository _userRepository;

        public SearchService(ITodoRepository todoRepository, IUserRepository userRepository)
        {
            _todoRepository = todoRepository;
            _userRepository = userRepository;
        }

        public SearchAllResult searchAll(string searchString)
        {
            return new SearchAllResult
            {
                Items = _todoRepository.GetMatchingItems(searchString),
                Lists = _todoRepository.GetMatchingLists(searchString),
                Users = _userRepository.GetMatchingUsers(searchString)
            };
        }

        public IEnumerable<TodoItem> searchItems(string searchString) { return _todoRepository.GetMatchingItems(searchString); }

        public IEnumerable<TodoItemList> searchLists(string searchString) { return _todoRepository.GetMatchingLists(searchString); }

        public IEnumerable<User> searchUsers(string searchString) { return _userRepository.GetMatchingUsers(searchString); }
    }

    public interface ISearchService
    {
        SearchAllResult searchAll(string searchString);
        IEnumerable<TodoItem> searchItems(string searchString);
        IEnumerable<TodoItemList> searchLists(string searchString);
        IEnumerable<User> searchUsers(string searchString);
    }
}
