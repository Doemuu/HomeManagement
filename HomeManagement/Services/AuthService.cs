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

        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest model, string ipAddress)
        {
            var user = await _databaseConnector.GetUserByUserName(model.Username);
            // return null if user not found
            if (user == null)
                return null;

            var authenticatedUser = await _databaseConnector.AuthenticateUser(model);

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = generateJwtToken(authenticatedUser);
            var refreshToken = generateRefreshToken(ipAddress);

            // save refresh token
            await _databaseConnector.AddRefreshToken(refreshToken, user);

            return new AuthenticationResponse(user, jwtToken, refreshToken.Token);
        }

        public AuthenticationResponse RefreshToken(string token, string ipAddress)
        {
            throw new NotImplementedException();
        }

        public bool RevokeToken(string token, string ipAddress)
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

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    ExpiresOn = DateTime.UtcNow.AddDays(7),
                    CreatedAt = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
    }
}
