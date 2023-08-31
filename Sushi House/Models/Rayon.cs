using System;
using System.Collections.Generic;

namespace Sushi_House.Models
{
    public partial class Rayon
    {
        public Rayon()
        {
            Users = new HashSet<User>();
        }

        public int RayonId { get; set; }
        public string? RayonName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
