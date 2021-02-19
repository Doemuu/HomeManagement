using HomeManagement.Entities;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Services
{
    public interface IAuthService
    {
        public Task<AuthenticationResponse> Authenticate(AuthenticationRequest model);
        public Task<AuthenticationResponse> RegisterUser(RegisterUserRequest user);
        public Task<AuthenticationResponse> RefreshToken(string token);
        public Task<bool> RevokeToken(string token);
    }
}
