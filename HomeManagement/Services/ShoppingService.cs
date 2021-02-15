using HomeManagement.Connector;
using HomeManagement.Entities;
using HomeManagement.Models;
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

        public async Task<ShoppingItem> GetShoppingItem(int id)
        {
            if (id < 0)
                return null;

            var result = await _databaseConnector.GetShoppingItemById(id);

            return result;
        }

        public async Task<ConnectorResult> EditShoppingItem(int id, ShoppingItem item)
        {
            if (id < 0)
                return new ConnectorResult { Success = false, Exception = "invalid_id" };

            var result = await _databaseConnector.EditShoppingItem(id, item);

            if (!result.Success)
                return new ConnectorResult { Success = false, Exception = result.Exception };

            return new ConnectorResult { Success = true };
        }

        public async Task<ConnectorResult> AddShoppingItem(ShoppingItem item)
        {
            var result = await _databaseConnector.AddShoppingItem(item);
            if (!result.Success)
                return new ConnectorResult { Success = false, Exception = result.Exception };

            return new ConnectorResult { Success = true };
        }
    }
}
