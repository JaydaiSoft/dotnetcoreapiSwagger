using System;
using System.Collections.Generic;
using DotNetcoreApiSwagger.Business;
using DotNetcoreApiSwagger.Model.Entity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotNetcoreApiSwagger.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SCGController : ControllerBase
    {
        private IBusinessManagement scgManager;
        public SCGController(IBusinessManagement scgManager)
        {
            this.scgManager = scgManager;
        }

        // GET api alive
        /// <summary>
        /// Get API Alive
        /// </summary>
        /// <remarks>This API will get the values.</remarks>
        [HttpGet, Route("IsAlive")]
        public ActionResult<bool> IsAlive()
        {
            return true;
        }

        // GET api alive
        /// <summary>
        /// X, 5, 9, 15, 23, Y, Z
        /// </summary>
        /// <remarks> function for finding X, Y, Z value </remarks>
        [HttpGet, Route("GetNumberSeries")]
        public ActionResult<string> GetNumberSeries()
        {
            string result = "";
            try
            {
                result = scgManager.CalculateNumberSeries();
                return Ok(result);
            }
            catch (Exception ex)
            {
                result = ex.GetBaseException().Message;
                return StatusCode(500, result);
            }

        }

        // GET local restaurants at Bangsue
        /// <summary>
        /// GET local restaurants at Bangsue
        /// </summary>
        /// <remarks> restaurants at Bangsue </remarks>
        [HttpGet, Route("GooglePlaceSearch")]
        public ActionResult<string> GooglePlaceSearch()
        {
            string result = "";
            try
            {
                result = scgManager.GooglePlaceSearch();
                return Ok(result);
            }
            catch (Exception ex)
            {
                result = ex.GetBaseException().Message;
                return StatusCode(500, result);
            }

        }

        // POST line notify
        /// <summary>
        /// Push Line message notify
        /// </summary>
        /// <remarks> push string line message </remarks>
        [HttpPost, Route("LineNotifyMessage")]
        public ActionResult<string> LineNotifyMessage(string message)
        {
            bool result = false;
            try
            {
                result = scgManager.LineNotifyMessage(message);
                if (result)
                {
                    return Ok(result);
                }
                return BadRequest();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.GetBaseException().Message);
            }

        }

        // GET Restaurants
        /// <summary>
        /// GET Restaurants
        /// </summary>
        /// <remarks> GET Restaurants </remarks>
        [HttpGet, Route("GetRestaurants")]
        public ActionResult<List<Restaurants>> GetRestaurants()
        {
            List<Restaurants> result;
            try
            {
                result = scgManager.GetRestaurants();
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }
                return BadRequest(new List<Restaurants>());

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.GetBaseException().Message);
            }

        }

        // POST RestaurantsNotify
        /// <summary>
        /// Push RestaurantsNotify
        /// </summary>
        /// <remarks> push string RestaurantsNotify </remarks>
        [HttpPost, Route("RestaurantsNotify")]
        public ActionResult<string> RestaurantsNotify()
        {
            List<bool> allSuccess = new List<bool>();
            try
            {
                string JsonStrRestaurants = scgManager.GooglePlaceSearch();
                JObject jObject = JObject.Parse(JsonStrRestaurants);
                List<Restaurants> Restaurants = new List<Restaurants>();
                bool IsSaveSuccess = scgManager.SaveRestaurants(jObject, ref Restaurants);
                if (IsSaveSuccess)
                {
                    if (Restaurants != null && Restaurants.Count > 0)
                    {
                        for (int i = 0; i < Restaurants.Count; i++)
                        {
                            string message = string.Format("ชื่อร้าน : {0} , ที่อยู่ : {1}, เรทราคา : {2}, เรทติ้งร้าน : {3}, สถานะร้าน : {4}",
                                Restaurants[i].Name,
                                Restaurants[i].Address,
                                Restaurants[i].Pricelevel,
                                Restaurants[i].Rating,
                                Restaurants[i].Available.HasValue ?
                                Restaurants[i].Available == true ? "Open" : "Close" : "Unknow");
                            bool canSend = scgManager.LineNotifyMessage(message);
                            allSuccess.Add(canSend);
                        }

                    }
                }

                if (allSuccess.FindAll(q => q.Equals(true)).Count == Restaurants.Count)
                {
                    return Ok(Restaurants);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.GetBaseException().Message);
            }

        }
    }
}