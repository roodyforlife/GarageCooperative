using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GarageCooperative.Models
{
    public class Cooperative
    {
        public int CooperativeId { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string PostCode { get; set; }
        [Required(ErrorMessage = "Should be not required")]
        [Phone(ErrorMessage = "Phone entered incorrectly")]
        public string Telephone { get; set; }
        public List<Row> Rows { get; set; }
    }
}
