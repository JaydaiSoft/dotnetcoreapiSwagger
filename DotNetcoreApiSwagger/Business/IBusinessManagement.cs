using DotNetcoreApiSwagger.Model.Entity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetcoreApiSwagger.Business
{
    public interface IBusinessManagement
    {
        string CalculateNumberSeries();

        bool LineNotifyMessage(string message);

        string GooglePlaceSearch();

        List<Restaurants> GetRestaurants();

        bool SaveRestaurants(JObject jObject, ref List<Restaurants> restaurants);
    }
}
