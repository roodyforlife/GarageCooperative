using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GarageCooperative.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string Name { get; set; }
        public bool HasSalary { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}
