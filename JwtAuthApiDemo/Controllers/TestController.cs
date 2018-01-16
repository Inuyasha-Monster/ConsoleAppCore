using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JwtAuthApiDemo.Controllers
{
    //[Authorize(Roles = "admin")] // 这种是直接参考返回Claims中Role给值参考而定
    [Authorize]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            string fuck = "empty";

            var first = HttpContext.User.Identities.FirstOrDefault();
            if (first != null)
            {
                fuck = first.Claims.Select(x => x.Type + " " + x.Value).Aggregate((x, y) =>
                        $"{x} -- {y}");
            }

            return Ok(fuck);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
