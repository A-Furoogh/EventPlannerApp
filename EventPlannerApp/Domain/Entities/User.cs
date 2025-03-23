using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Domain.Entities
{
    public class User 
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
        public required string Password { get; set; }
        public List<int>? EventsIds { get; set; }
    }
}
