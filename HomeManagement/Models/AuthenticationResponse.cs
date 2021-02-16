using HomeManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Models
{
    public class AuthenticationResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }

        public AuthenticationResponse(User user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.UserName;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
