using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageCooperative.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Lastname { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string PassportNumber { get; set; }
        public string Address { get; set; }
        public int Salary { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public List<Membership> Memberships { get; set; }
    }
}
