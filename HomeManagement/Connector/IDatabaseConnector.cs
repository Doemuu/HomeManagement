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
        public Task<Note> FetchNoteByTitle(string title);
        public Task<ConnectorResult> CreateNote(Note note);
        public Task<Note> GetNoteById(int id);
    }
}
