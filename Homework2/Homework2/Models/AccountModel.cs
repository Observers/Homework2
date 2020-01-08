using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Homework2.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountModel : ControllerBase
    {
        public string Username { get; set; }

        public string Password { get; set; }
        /*
        // GET: api/AccountModel
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/AccountModel/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/AccountModel
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/AccountModel/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
