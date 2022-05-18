using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Models
{
    public class OrderDetailsData
    {
        public int? Id { get; set; }
        public int? OrderId { get; set; }
        public int FoodId { get; set; }
        public int Qty { get; set; }
        public string Location { get; set; }
        public string Tracker { get; set; } 
    }
}
