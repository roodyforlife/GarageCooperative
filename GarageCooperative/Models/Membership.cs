using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GarageCooperative.Models
{
    public class Membership
    {
        public int MembershipId { get; set; }
        public Garage Garage { get; set; }
        public int GarageId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        public DateTime OwnStart { get; set; }
        public Nullable<DateTime> OwnEnd { get; set; }
    }
}
