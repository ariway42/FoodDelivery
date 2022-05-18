using System;
using System.Collections.Generic;

namespace FoodServices.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int FoodId { get; set; }
        public int Qty { get; set; }
        public string Location { get; set; } = null!;
        public string Tracker { get; set; } = null!;

        public virtual Food Food { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}
