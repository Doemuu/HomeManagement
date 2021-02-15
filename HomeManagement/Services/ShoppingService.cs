using HomeManagement.Connector;
using HomeManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Services
{
    public class ShoppingService
    {
        private readonly IDatabaseConnector _databaseConnector;

        public ShoppingService(IDatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector;
        }

        public async Task<List<ShoppingItem>> GetShoppingList()
        {
            var result = await _databaseConnector.GetShoppingList();

            return result;
        }
    }
}
