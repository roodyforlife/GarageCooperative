using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GarageCooperative.Models
{
    public class Fee
    {
        public int FeeId { get; set; }
        public Garage Garage { get; set; }
        public int GarageId { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        public int Payment { get; set; }
    }
}
