using DotNetcoreApiSwagger.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetcoreApiSwagger.Repository
{
    public interface IScgRepository
    {
        List<Restaurants> GetRestaurants();
    }
}
