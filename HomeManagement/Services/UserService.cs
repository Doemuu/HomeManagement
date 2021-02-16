using HomeManagement.Connector;
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
    }
}
