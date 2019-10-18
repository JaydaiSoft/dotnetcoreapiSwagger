using DotNetcoreApiSwagger.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetcoreApiSwagger.Repository
{
    public class ScgRepository : IScgRepository
    {
        private IScgContext context;

        public ScgRepository()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var ConnectionString = configuration.GetSection("AppSettings:ConnectionString").Value;
            var optionsBuilder = new DbContextOptionsBuilder<ScgContext>();
            optionsBuilder.UseSqlServer(ConnectionString);
            context = new ScgContext(optionsBuilder.Options);
        }

        public List<Restaurants> GetRestaurants()
        {
            return context.RestaurantEntity.ToList();
        }
    }
}
