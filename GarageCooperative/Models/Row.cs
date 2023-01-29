﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageCooperative.Models
{
    public class Row
    {
        public int RowId { get; set; }
        public int RowNumber { get; set; }
        public int MaxGarageCount { get; set; }
        public Cooperative Cooperative { get; set; }
        public int CooperativeId { get; set; }
        public List<Garage> Garages { get; set; }
    }
}