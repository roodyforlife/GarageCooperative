using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageCooperative.Models
{
    public class Cooperative
    {
        public int CooperativeId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string Telephone { get; set; }
        public List<Row> Rows { get; set; }
    }
}
