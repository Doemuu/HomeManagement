using HomeManagement.Connector;
using HomeManagement.Entities;
using HomeManagement.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HomeManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppSettings _appSettings;
        private readonly IDatabaseConnector _databaseConnector;

        public AuthService(IOptions<AppSettings> appSettings, IDatabaseConnector databaseConnector)
        {
            _appSettings = appSettings.Value;
            _databaseConnector = databaseConnector;
        }

        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest model)
        {
            var user = await _databaseConnector.GetUserByUserName(model.Username);
            // return null if user not found
            if (user == null)
                return null;

            bool verify = BCrypt.Net.BCrypt.Verify(model.Password, user.UserPassword);
            if (!verify)
                return null;

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = generateJwtToken(user);
            var refreshToken = generateRefreshToken(user).Result;

            // save refresh token
            await _databaseConnector.SaveRefreshToken(new RefreshToken { Token = refreshToken, IsRevoked = false, CreatedAt = DateTime.UtcNow});

            return new AuthenticationResponse(user, jwtToken, refreshToken);
        }

        public AuthenticationResponse RefreshToken(string token)
        {
            throw new NotImplementedException();
        }

        public bool RevokeToken(string token)
        {
            throw new NotImplementedException();
        }

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<string> generateRefreshToken(User user)
        {
            var result = await _databaseConnector.GenerateRefreshToken(new RefreshToken { UserId = user.Id, ExpiresOn = DateTime.UtcNow});
            if (!result.Success)
                return result.Exception;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, result.LastId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
