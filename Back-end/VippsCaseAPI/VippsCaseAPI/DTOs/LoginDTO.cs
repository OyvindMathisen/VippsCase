using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VippsCaseAPI.DTOs
{
    public class LoginDTO
    {
        public LoginDTO(string Token, string Message)
        {
            this.Token = Token;
            this.Message = Message;
        }

        public LoginDTO(string Message)
        {
            this.Message = Message;
        }

        public string Token { get; set; }
        public string Message { get; set; }
    }
}
