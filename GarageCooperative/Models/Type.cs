using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageCooperative.Models
{
    public class Type
    {
        public int TypeId { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; } // per month
        public int GarbadgeCost { get; set; }
        public int WaterCost { get; set; }
        public int FloorNumber { get; set; }
        public List<TypeGarage> TypeGarages { get; set; }
    }
}
