﻿using DotNetcoreApiSwagger.Model.Entity;
using System.Collections.Generic;
using System.Linq;

namespace DotNetcoreApiSwagger.Repository
{
    public class ScgRepository : IScgRepository
    {
        private IScgContext context;

        public ScgRepository(IScgContext context)
        {
            this.context = context;
        }

        public List<Restaurants> GetRestaurants()
        {
            return context.RestaurantEntity.ToList();
        }

        public void SaveRestaurants(List<Restaurants> restaurants)
        {
            context.RestaurantEntity.AddRange(restaurants);
        }

        public void Commit()
        {
            context.SaveChanges();
        }
    }
}
