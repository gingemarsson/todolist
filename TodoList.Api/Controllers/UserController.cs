using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoList.Api.Repositories;
using TodoList.Api.Requests;
using TodoList.Api.Services;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _service;

        public UserController(IUserService userService)
        {
            _service = userService;
        }

        /// <summary>
        /// Get a list of all users.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            return _service.GetAllUsers().ToList();
        }

        /// <summary>
        /// Get a spefified user.
        /// </summary>
        /// <param name="id">The id of the user to get.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<User> GetUser([FromRoute]Guid id)
        {
            var res = _service.GetUser(id);

            if (res == null)
                return NotFound();

            return _service.GetUser(id);
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="userRequest">The user to create.</param>
        /// <returns></returns>
        [HttpPut]
        public ActionResult<User> CreateUser([FromBody]UserRequest userRequest)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = userRequest.Name
            };

            return _service.CreateUser(user);
        }
    }
}