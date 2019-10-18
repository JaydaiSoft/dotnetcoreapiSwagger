using DotNetcoreApiSwagger.Model.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetcoreApiSwagger.Repository
{
    public interface IScgContext
    {
        DbSet<Restaurants> RestaurantEntity { get; set; }
    }
}
