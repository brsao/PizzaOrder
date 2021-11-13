using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string PizzaType { get; set; }
        public string PizzaSize { get; set; }
        public decimal Price { get; set; }

    }
}