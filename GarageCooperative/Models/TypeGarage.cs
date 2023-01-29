using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageCooperative.Models
{
    public class TypeGarage
    {
        public int TypeGarageId { get; set; }
        public Garage Garage { get; set; }
        public int GarageId { get; set; }
        public Type Type { get; set; }
        public int TypeId { get; set; }
    }
}
