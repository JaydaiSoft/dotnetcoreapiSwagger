using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetcoreApiSwagger.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SCGController : ControllerBase
    {
        // GET api/values
        /// <summary>
        /// Get API Value
        /// </summary>
        /// <remarks>This API will get the values.</remarks>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}