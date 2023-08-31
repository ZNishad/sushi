using System;
using System.Collections.Generic;

namespace Sushi_House.Models
{
    public partial class Operator
    {
        public Operator()
        {
            Users = new HashSet<User>();
        }

        public int Operatorid { get; set; }
        public int? OperatorList { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
