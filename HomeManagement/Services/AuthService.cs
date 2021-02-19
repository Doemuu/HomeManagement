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
        private readonly UserService _userService;

        public AuthService(IOptions<AppSettings> appSettings, IDatabaseConnector databaseConnector, UserService userService)
        {
            _appSettings = appSettings.Value;
            _databaseConnector = databaseConnector;
            _userService = userService;
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

            return new AuthenticationResponse(user, jwtToken, refreshToken);
        }

        public async Task<AuthenticationResponse> RegisterUser(RegisterUserRequest user)
        {
            var result = await _userService.RegisterUser(user);
            if (result == null)
                return null;

            var jwtToken = generateJwtToken(result);
            var refreshToken = generateRefreshToken(result).Result;

            return new AuthenticationResponse(result, jwtToken, refreshToken);
        }

        public async Task<AuthenticationResponse> RefreshToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var id = jwt.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
            try
            {
                var intId = Int32.Parse(id);
                var result = await _databaseConnector.GetTokenById(intId);
                var comparison = DateTime.Compare(DateTime.UtcNow, result.ExpiresOn);
                if (result.IsRevoked || comparison <= 0 || result == null)
                    return null;

                var user = await _databaseConnector.GetUserById(result.UserId);
                if (user == null)
                    return null;

                var jwtToken = generateJwtToken(user);

                return new AuthenticationResponse(user, jwtToken, token);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> RevokeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var id = jwt.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
            try
            {
                var intId = Int32.Parse(id);
                var result = await _databaseConnector.GetTokenById(intId);
                var comparison = DateTime.Compare(DateTime.UtcNow, result.ExpiresOn);
                if (result.IsRevoked || comparison <= 0 || result == null)
                    return false;

                var revokeToken = await _databaseConnector.RevokeToken(intId);
                if (!revokeToken.Success)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("abcId", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<string> generateRefreshToken(User user)
        {
            var result = await _databaseConnector.GenerateRefreshToken(new RefreshToken { UserId = user.Id, ExpiresOn = DateTime.UtcNow.AddDays(7)});
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
