using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SSoft.IoT.MVC.Areas.CultivoApi.Controllers
{
    public class IndexController : ApiController
    {
        // GET api/index
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/index/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/index
        public void Post([FromBody]string value)
        {
        }

        // PUT api/index/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/index/5
        public void Delete(int id)
        {
        }
    }
}
