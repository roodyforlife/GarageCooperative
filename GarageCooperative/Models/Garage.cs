using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GarageCooperative.Models
{
    public class Garage
    {
        public int GarageId { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        public int Number { get; set; }
        public int GarageSpace { get; set; }
        public int CarsCapacity { get; set; }
        public Row Row { get; set; }
        public int RowId { get; set; }
        public Type Type { get; set; }
        public int TypeId { get; set; }
        public List<Membership> Memberships { get; set; }
        public List<Fee> Fees { get; set; }
    }
}
