using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetcoreApiSwagger.Model.Entity
{
    public class Restaurants
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int? Pricelevel { get; set; }
        public decimal? Rating { get; set; }
        public bool? Available { get; set; }
    }
}
