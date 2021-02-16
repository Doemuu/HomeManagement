using HomeManagement.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Services
{
    public class AuthService
    {
        private readonly IDatabaseConnector _databaseConnector;

        public AuthService(IDatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector;
        }
    }
}
