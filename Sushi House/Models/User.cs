using System;
using System.Collections.Generic;

namespace Sushi_House.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserSurname { get; set; }
        public int? UserStatId { get; set; }
        public int? UserGenderId { get; set; }
        public DateTime? UserBirth { get; set; }
        public int? UserOperId { get; set; }
        public string? UserNumber { get; set; }
        public string? UserPassword { get; set; }
        public string? UserMail { get; set; }
        public int? UserRayonId { get; set; }
        public string? UserAdress { get; set; }
        public string? UserDescription { get; set; }

        public virtual Gender? UserGender { get; set; }
        public virtual Operator? UserOper { get; set; }
        public virtual Rayon? UserRayon { get; set; }
        public virtual Stat? UserStat { get; set; }
    }
}
