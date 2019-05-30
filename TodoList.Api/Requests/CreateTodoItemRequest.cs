using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Api.Requests
{
    public class TTRequest
    {
        public string Name { get; set; }
        public string Description { get; set; } = "";
        public bool Done { get; set; } = false;
    }
}
