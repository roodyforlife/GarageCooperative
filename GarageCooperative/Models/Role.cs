using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageCooperative.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public bool HasSalary { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}
