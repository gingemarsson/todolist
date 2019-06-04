using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Api.Entities
{
    public class SearchAllResult
    {
        public IEnumerable<TodoItem> Items { get; set; }
        public IEnumerable<TodoItemList> Lists { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
