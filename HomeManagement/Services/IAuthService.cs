using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Services
{
    public interface IAuthService
    {
        public Task<AuthenticationResponse> Authenticate(AuthenticationRequest model, string ipAddress);
        public AuthenticationResponse RefreshToken(string token, string ipAddress);
        public bool RevokeToken(string token, string ipAddress);
    }
}
