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
                    var result = await sql.QueryAsync<ShoppingItem>("SELECT * FROM ShoppingList WHERE IsDeleted = false");
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

        public async Task<ShoppingItem> GetShoppingItemById(int id)
        {
            using (var sql = GetSqlConnection())
            {
                try
                {
                    var result = await sql.QueryFirstAsync<ShoppingItem>("SELECT * FROM ShoppingList WHERE Id = @Id", new { Id = id });
                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<ConnectorResult> AddShoppingItem(ShoppingItem item)
        {
            using (var sql = GetSqlConnection())
            {
                try
                {
                    var result = await sql.ExecuteAsync("INSERT INTO ShoppingList (ItemName, Section, Amount, Priority, IsBought, IsFavourite, IsDeleted) " +
                        "VALUES (@ItemName, @Section, @Amount, @Priority, @IsBought, @IsFavourite, @IsDeleted)",
                        new
                        {
                            ItemName = item.ItemName,
                            Section = item.Section,
                            Priority = item.Priority,
                            IsBought = item.IsBought,
                            IsFavourite = item.IsFavourite,
                            IsDeleted = item.IsDeleted
                        });
                    return new ConnectorResult { Success = true };
                }
                catch (Exception ex)
                {
                    return new ConnectorResult { Success = false, Exception = ex.Message };
                }
            }
        }

        public async Task<ConnectorResult> EditShoppingItem(int id, ShoppingItem item)
        {
            using (var sql = GetSqlConnection())
            {
                try
                {
                    var result = await sql.ExecuteAsync("UPDATE ShoppingList SET ItemName = @ItemName, " +
                        "Section = @Section, Amount = @Amount, Priority = @Priority, IsBought = @IsBought, IsFavourite = @IsFavourite, IsDeleted = @IsDeleted",
                        new
                        {
                            ItemName = item.ItemName,
                            Section = item.Section,
                            Amount = item.Amount,
                            Priority = item.Priority,
                            IsBought = item.IsBought,
                            IsFavourite = item.IsFavourite,
                            IsDeleted = item.IsDeleted
                        });
                    return new ConnectorResult { Success = true };
                }
                catch (Exception ex)
                {
                    return new ConnectorResult { Success = false, Exception = ex.Message };
                }
            }
        }

        public async Task<User> GetUserByUserName(string UserName)
        {
            using (var sql = GetSqlConnection())
            {
                try
                {
                    var result = await sql.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE UserName = @UserName", new { UserName = UserName });
                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<User> RegisterUser(RegisterUserRequest user)
        {
            using (var sql = GetSqlConnection())
            {
                try
                {
                    var returnUser = new User { FirstName = user.FirstName, LastName = user.LastName, UserName = user.UserName, UserPassword = user.UserPassword };

                    var result = await sql.ExecuteScalarAsync<int>("INSERT INTO Users (FirstName, LastName, UserName, UserPassword) VALUES (@FirstName, @LastName, @UserName, @UserPassword);" +
                        "SELECT LAST_INSERT_ID()",
                        new { FirstName = user.FirstName, LastName = user.LastName, UserName = user.UserName, UserPassword = user.UserPassword });

                    returnUser.Id = result;
                    return returnUser;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<ConnectorResult> EditPassword(string username, string password)
        {
            using (var sql = GetSqlConnection())
            {
                try
                {
                    var result = await sql.ExecuteAsync("UPDATE Users SET UserPassword = @UserPassword WHERE UserName = @UserName", new { UserPassword = password, UserName = username });
                    return new ConnectorResult { Success = true };
                }
                catch (Exception ex)
                {
                    return new ConnectorResult { Success = false, Exception = ex.Message };
                }
            }
        }

        public async Task<ConnectorResult> EditUserData(string username, User user)
        {
            using (var sql = GetSqlConnection())
            {
                try
                {
                    var result = await sql.ExecuteAsync("UPDATE Users SET FirstName = @FirstName, LastName = @LastName, UserName = @UserName",
                        new { FirstName = user.FirstName, LastName = user.LastName, UserName = user.UserName });
                    return new ConnectorResult { Success = true };
                }
                catch (Exception ex)
                {
                    return new ConnectorResult { Success = false, Exception = ex.Message };
                }
            }
        }

        public async Task<ConnectorResult> GenerateRefreshToken(RefreshToken token)
        {
            using (var sql = GetSqlConnection())
            {
                try
                {
                    var result = await sql.ExecuteScalarAsync<int>("INSERT INTO RefreshToken (UserId, ExpiresOn) VALUES" +
                        " (@UserId, @ExpiresOn);" +
                        "SELECT LAST_INSERT_ID()", 
                        new { UserId = token.UserId, ExpiresOn = token.ExpiresOn});

                    return new ConnectorResult { Success = true, LastId = result };
                }
                catch (Exception ex)
                {
                    return new ConnectorResult { Success = false, Exception = ex.Message };
                }
            }
        }

        public async Task<RefreshToken> GetTokenById(int id)
        {
            using (var sql = GetSqlConnection())
            {
                try
                {
                    var result = await sql.QuerySingleOrDefaultAsync<RefreshToken>("SELECT * FROM RefreshToken WHERE Id = @Id", new { Id = id});
                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<User> GetUserById(int id)
        {
            using (var sql = GetSqlConnection())
            {
                try
                {
                    var result = await sql.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id });
                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<ConnectorResult> RevokeToken(int id)
        {
            using (var sql = GetSqlConnection())
            {
                try
                {
                    var result = await sql.ExecuteAsync("UPDATE RefreshToken SET IsRevoked = true");
                    return new ConnectorResult { Success = true };
                }
                catch (Exception ex)
                {
                    return new ConnectorResult { Success = false, Exception = ex.Message };
                }
            }
        }
    }
}
