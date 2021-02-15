using Dapper;
using HomeManagement.Entities;
using HomeManagement.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Connector.Database
{
    public class DatabaseConnector : IDatabaseConnector
    {
        public MySqlConnection GetSqlConnection()
        {
            return new MySqlConnection(Environment.GetEnvironmentVariable("ConnectionString"));
        }

        public async Task<List<ShoppingItem>> GetShoppingList()
        {
            using (var sql = GetSqlConnection())
            {
                try
                {
                    var result = await sql.QueryAsync<ShoppingItem>("SELECT * FROM ShoppingList");
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<Note> FetchNoteByTitle(string title)
        {
            using (var sql = GetSqlConnection())
            {
                try
                {
                    var result = await sql.QueryFirstAsync<Note>("SELECT * FROM Note WHERE title = @title", new { title = title });
                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<ConnectorResult> CreateNote(Note note)
        {
            using (var sql = GetSqlConnection())
            {
                try
                {
                    var result = await sql.ExecuteAsync("INSERT INTO Note (NoteTitle, UploadDate, Ending, IsDeleted) VALUES (@NoteTitle, @UploadDate, @Ending, @IsDeleted)",
                        new { NoteTitle = note.NoteTitle, UploadDate = note.UploadDate, Ending = note.Ending, IsDeleted = note.IsDeleted });

                    return new ConnectorResult { Success = true };
                }
                catch (Exception ex)
                {
                    return new ConnectorResult { Success = false, Exception = ex.Message };
                }
            }
        }

        public async Task<Note> GetNoteById(int id)
        {
            using (var sql = GetSqlConnection())
            {
                try
                {
                    var result = await sql.QueryFirstAsync<Note>("SELECT * FROM Note WHERE NoteId = @Id", new { Id = id });

                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
