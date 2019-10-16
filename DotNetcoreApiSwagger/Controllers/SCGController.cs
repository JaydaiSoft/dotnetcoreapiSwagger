using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetcoreApiSwagger.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetcoreApiSwagger.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SCGController : ControllerBase
    {
        private IBusinessManagement _businessManagement;
        public SCGController()
        {
            _businessManagement = new BusinessManagement();
        }

        // GET api/values
        /// <summary>
        /// Get API Value
        /// </summary>
        /// <remarks>This API will get the values.</remarks>
        [HttpGet, Route("TestGetValue")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
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
                result = _businessManagement.CalculateNumberSeries();
                return Ok(result);
            }
            catch(Exception ex)
            {
                result = ex.GetBaseException().Message;
                return StatusCode(500,result);
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
                result = _businessManagement.LineNotifyMessage(message);
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
    }
}