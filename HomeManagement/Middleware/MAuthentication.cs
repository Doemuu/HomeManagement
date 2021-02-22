using HomeManagement.Connector.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HomeManagement.Middleware
{
    public class MAuthentication
    {
        private readonly RequestDelegate _next;

        public MAuthentication(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            if (token != null)
            {
                token = token.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                var id = jwt.Claims.First(claim => claim.Type == "abcId").Value;

                try
                {
                    var intId = Int32.Parse(id);
                    var admin = false;

                    DatabaseConnector dbconnector = new DatabaseConnector();

                    var user = await dbconnector.GetUserById(intId);
                    if (user != null)
                        admin = user.IsAdmin;

                    context.Items.Add("user", admin);
                }
                catch (Exception ex)
                {
                    throw new Exception("invalid_token");
                }
            }

            await _next(context);

        }
    }
}
