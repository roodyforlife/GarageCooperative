using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageCooperative.Models
{
    public class Garage
    {
        public int GarageId { get; set; }
        public int Number { get; set; }
        public int GarageSpace { get; set; }
        public int CarsCapacity { get; set; }
        public Row Row { get; set; }
        public int RowId { get; set; }
    }
}
