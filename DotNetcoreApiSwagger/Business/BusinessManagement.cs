﻿using DotNetcoreApiSwagger.Model.Entity;
using DotNetcoreApiSwagger.Repository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DotNetcoreApiSwagger.Business
{
    public class BusinessManagement : IBusinessManagement
    {
        private IScgRepository repository;
        public BusinessManagement(IScgRepository repository)
        {
            this.repository = repository;
            //Get AppSetting from appsettings.json
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            lineNotifyUrl = configuration.GetSection("LineUrl").Value;
            token = configuration.GetSection("Token").Value;
            googlePlaceSearchApi = configuration.GetSection("GooglePlaceSearchUrl").Value;
            googleToken = configuration.GetSection("GoogleToken").Value;
        }

        private string token { get; set; }

        private string lineNotifyUrl { get; set; }

        private string googlePlaceSearchApi { get; set; }

        private string googleToken { get; set; }

        public string CalculateNumberSeries()
        {
            int[] numberSeries = new int[7];
            numberSeries = RegonitionPattern(ref numberSeries);
            return string.Format("X, 5, 9, 15, 23, Y, Z Result : X = {0}, Y = {1}, Z = {2}"
                , numberSeries[0], numberSeries[5], numberSeries[6]);
        }

        private int[] RegonitionPattern(ref int[] numberSeries)
        {
            for (int i = 0; i <= numberSeries.Length - 1; i++)
            {
                if (i == 0)
                {
                    numberSeries[0] = 3;
                }
                else
                {
                    int n = i + 1;
                    numberSeries[i] = numberSeries[i - 1] + 2 * (n - 1);
                }
            }
            return numberSeries;
        }

        public bool LineNotifyMessage(string message)
        {
            bool IsSuccess = false;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(lineNotifyUrl);
                var postData = string.Format("message={0}", message);
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);

                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    IsSuccess = true;
                }
                return IsSuccess;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return IsSuccess;
            }
        }

        public string GooglePlaceSearch()
        {
            try
            {
                googlePlaceSearchApi = googlePlaceSearchApi + "restaurants+in+Bangsue" + "&key=" + googleToken;
                var request = (HttpWebRequest)WebRequest.Create(googlePlaceSearchApi);

                request.Method = "GET";
                request.ContentType = "application/json; charset=UTF-8";

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return responseString;
                }
                return responseString;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ex.GetBaseException().Message;
            }
        }

        public List<Restaurants> GetRestaurants()
        {
            List<Restaurants> entities = repository.GetRestaurants();
            if (entities != null && entities.Any())
                return entities;
            return new List<Restaurants>();
        }

        public bool SaveRestaurants(JObject jObject, ref List<Restaurants> restaurants)
        {
            bool IsSaveSuccess = false;
            try
            {
                Restaurants restaurant = null;
                JArray restaurants_count = (JArray)jObject["results"];
                int count = restaurants_count.Count;
                for (int i = 0; i < count; i++)
                {
                    restaurant = new Restaurants();
                    restaurant.Address = (string)jObject["results"][i]["formatted_address"];
                    restaurant.Name = (string)jObject["results"][i]["name"];
                    restaurant.Pricelevel = (int?)jObject["results"][i]["price_level"] != null ? (int?)jObject["results"][i]["price_level"] : 0;
                    restaurant.Rating = (decimal?)jObject["results"][i]["rating"];
                    restaurant.Available = jObject["results"][i]["opening_hours"] != null ? (bool?)jObject["results"][i]["opening_hours"]["open_now"] : null;
                    restaurants.Add(restaurant);
                }
                repository.SaveRestaurants(restaurants);
                repository.Commit();
                IsSaveSuccess = true;
                return IsSaveSuccess;
            }
            catch (Exception ex)
            {
                IsSaveSuccess = false;
                return IsSaveSuccess;
            }

        }
    }
}
