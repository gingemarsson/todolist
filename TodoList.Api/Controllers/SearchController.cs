using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoList.Api.Entities;
using TodoList.Api.Services;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        /// <summary>
        /// Search for both items, lists and users at the same time.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<SearchAllResult> SearchAll([FromQuery] string query)
        {
            return _searchService.searchAll(query);
        }

        /// <summary>
        /// Search for todo list items.
        /// </summary>
        /// <param name="query">The search query. All items whoose name or description contain this string will be returned.</param>
        /// <returns></returns>
        [HttpGet("items")]
        public ActionResult<IEnumerable<TodoItem>> SearchItems([FromQuery] string query)
        {
            return _searchService.searchItems(query).ToList();
        }

        /// <summary>
        /// Search for todo lists.
        /// </summary>
        /// <param name="query">The search query. All lists whoose name contain this string will be returned.</param>
        /// <returns></returns>
        [HttpGet("lists")]
        public ActionResult<IEnumerable<TodoItemList>> SearchLists([FromQuery] string query)
        {
            return _searchService.searchLists(query).ToList();
        }

        /// <summary>
        /// Search for users.
        /// </summary>
        /// <param name="query">The search query. All users whoose name contain this string will be returned.</param>
        /// <returns></returns>
        [HttpGet("users")]
        public ActionResult<IEnumerable<User>> SearchUsers([FromQuery] string query)
        {
            return _searchService.searchUsers(query).ToList();
        }
    }
}