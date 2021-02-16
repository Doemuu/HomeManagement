using HomeManagement.Entities;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Connector
{
    public interface IDatabaseConnector
    {
        public Task<List<ShoppingItem>> GetShoppingList();
        public Task<ShoppingItem> GetShoppingItemById(int id);
        public Task<ConnectorResult> AddShoppingItem(ShoppingItem item);
        public Task<ConnectorResult> EditShoppingItem(int id, ShoppingItem item);
        public Task<Note> FetchNoteByTitle(string title);
        public Task<ConnectorResult> CreateNote(Note note);
        public Task<Note> GetNoteById(int id);
        public Task<User> GetUserByUserName(string UserName);
        public Task<ConnectorResult> AddRefreshToken(RefreshToken token, User user);
    }
}
