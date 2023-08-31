using System;
using System.Collections.Generic;

namespace Sushi_House.Models
{
    public partial class Gender
    {
        public Gender()
        {
            Users = new HashSet<User>();
        }

        public int GenderId { get; set; }
        public string? GenderName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
