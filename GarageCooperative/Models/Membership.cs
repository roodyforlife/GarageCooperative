using System;
using System.Collections.Generic;
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
        public DateTime OwnStart { get; set; }
        public DateTime OwnEnd { get; set; }
    }
}
