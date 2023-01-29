using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GarageCooperative.Models
{
    public class Type
    {
        public int TypeId { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        public int Cost { get; set; } // per month
        [Required(ErrorMessage = "Should be not required")]
        public int GarbadgeCost { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        public int WaterCost { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        public int FloorNumber { get; set; }
        public List<TypeGarage> TypeGarages { get; set; }
    }
}
