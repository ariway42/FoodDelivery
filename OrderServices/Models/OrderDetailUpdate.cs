using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Models
{
    public class OrderDetailUpdate
    {
        public int? Id { get; set; }
        public int FoodId { get; set; }
        public int Qty { get; set; }
       
    }
}
