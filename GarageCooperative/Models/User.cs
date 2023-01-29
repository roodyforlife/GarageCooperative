using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GarageCooperative.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        [Phone(ErrorMessage = "Phone entered incorrectly")]
        public string Telephone { get; set; }
        [EmailAddress(ErrorMessage = "Email entered incorrectly")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        public string PassportNumber { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string Address { get; set; }
        public int Salary { get; set; }
        public Role Role { get; set; }
        public int RoleId { get; set; }
        public List<Membership> Memberships { get; set; }
    }
}
