using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using VippsCaseAPI.DataAccess;
using VippsCaseAPI.Models;

namespace VippsCaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DBContext _context;

        public AuthController(DBContext context)
        {
            _context = context;
        }

        [HttpPost("createUser")]
        public async Task<ActionResult> CreateUser([FromBody]JObject data)
        {
            //TODO: Validate input

            /*_context.users.Add(user);
            await _context.SaveChangesAsync();*/

            //TODO: Exception Handling

            User user = data["userData"].ToObject<User>();

            string password = data["password"].ToString();

            return Ok(user);
        }
    }
}
