using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            //TODO: Exception Handling

            //User generation

            User user = data["userData"].ToObject<User>();

            _context.users.Add(user);

            //Paaword generation

            string password = data["password"].ToString();

            Password p = new Password();

            p.Salt = generateSalt();

            p.PasswordHash = ComputeSha512Hash(password + p.Salt);

            p.UserId = user.UserId;

            p.Active = true;

            _context.passwords.Add(p);

            await _context.SaveChangesAsync();

            return Ok(p);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody]JObject data)
        {
            //TODO: Exception handling
            //TODO: Input validation

            string email = data["email"].ToString();
            string password = data["password"].ToString();

            User u = await _context.users.FirstOrDefaultAsync(x => x.Email == email);

            Password p = await _context.passwords.FirstOrDefaultAsync(x => x.UserId == u.UserId);

            string hashedPassword = ComputeSha512Hash(password + p.Salt);

            if (hashedPassword == p.PasswordHash)
            {
                return Ok("User Validated!");
            }
            else
            {
                return StatusCode(501);
            }
        }

        //TODO: Seperation of concern
        //src: https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
        public string generateSalt()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 9)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //src: https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/
        static string ComputeSha512Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA512 sha256Hash = SHA512.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
