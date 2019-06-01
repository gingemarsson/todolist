using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Repositories;

namespace TodoList.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository userRepository)
        {
            _repository = userRepository;
        }

        public User GetUser(Guid id) { return _repository.GetUser(id); }

        public IEnumerable<User> GetAllUsers() { return _repository.GetAllUsers(); }

        public User CreateUser(User user) { return _repository.SaveUser(user); }
    }

    public interface IUserService
    {
        User GetUser(Guid id);
        IEnumerable<User> GetAllUsers();
        User CreateUser(User user);

    }
}
