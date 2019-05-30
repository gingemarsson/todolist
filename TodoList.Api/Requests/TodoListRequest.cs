using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Api.Requests
{
    public class TodoListRequest
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
    }
}
