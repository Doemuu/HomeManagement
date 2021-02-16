using HomeManagement.Connector;
using HomeManagement.Entities;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Services
{
    public class UserService
    {
        private readonly IDatabaseConnector _databaseConnector;

        public UserService(IDatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector;
        }

        public async Task<ConnectorResult> RegisterUser(User user)
        {
            if (user.FirstName.Length <= 0)
                return new ConnectorResult { Success = false, Exception = "invalid_first_name" };
            if (user.LastName.Length <= 0)
                return new ConnectorResult { Success = false, Exception = "invalid_last_name" };
            if (user.UserName.Length <= 0)
                return new ConnectorResult { Success = false, Exception = "invalid_username" };
            if (user.UserPassword.Length < 8)
                return new ConnectorResult { Success = false, Exception = "password_length" };

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.UserPassword);
            user.UserPassword = passwordHash;

            var result = await _databaseConnector.RegisterUser(user);
            if (!result.Success)
                return new ConnectorResult { Success = false, Exception = result.Exception };

            return new ConnectorResult { Success = true };
        }

        public async Task<User> GetUserByUserName(string username)
        {
            if (username.Length <= 0)
                return null;

            var result = await _databaseConnector.GetUserByUserName(username);

            return result;
        }

        public async Task<ConnectorResult> ChangePassword(string username, string password)
        {
            if (username.Length <= 0)
                return new ConnectorResult { Success = false, Exception = "invalid_username" };

            if (password.Length <= 8)
                return new ConnectorResult { Success = false, Exception = "invalid_password" };

            var fetchedUser = await _databaseConnector.GetUserByUserName(username);

            if (fetchedUser == null)
                return new ConnectorResult { Success = false, Exception = "inexistent_user" };

            bool verfied = BCrypt.Net.BCrypt.Verify(password, fetchedUser.UserPassword);
            if (verfied)
                return new ConnectorResult { Success = false, Exception = "same_password" };

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var result = await _databaseConnector.EditPassword(username, password);
            if (!result.Success)
                return new ConnectorResult { Success = false, Exception = result.Exception };

            return new ConnectorResult { Success = true };
        }
        public async Task<ConnectorResult> EditUserData(User user)
        {
            if (user.UserName.Length <= 0)
                return new ConnectorResult { Success = false, Exception = "invalid_username" };

            var result = await _databaseConnector.EditUserData(user.UserName, user);
            if (!result.Success)
                return new ConnectorResult { Success = false, Exception = result.Exception };

            return new ConnectorResult { Success = true };
        }
    }
}
