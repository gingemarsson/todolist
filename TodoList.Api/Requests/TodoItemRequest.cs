using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Api.Requests
{
    public class TodoItemRequest
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(1024)]
        public string Description { get; set; } = "";
        public bool Done { get; set; } = false;
    }
}
