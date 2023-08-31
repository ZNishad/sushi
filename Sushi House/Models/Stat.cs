using System;
using System.Collections.Generic;

namespace Sushi_House.Models
{
    public partial class Stat
    {
        public Stat()
        {
            Users = new HashSet<User>();
        }

        public int StatId { get; set; }
        public string? StatName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
