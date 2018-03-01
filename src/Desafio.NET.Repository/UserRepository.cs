using Desafio.NET.Domain.Entities;
using Desafio.NET.Repository.Abstractions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.NET.Repository
{
    /// <summary>
    /// Implementation for user repository contracts
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        /// <inheritdoc />
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <inheritdoc />
        public async Task<User> CreateUserAsync(User user)
        {
            if (user.Id != 0) throw new ArgumentException("User ID need be equal 0!");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = @"INSERT INTO [dbo].[User] 
                                            (Name, Email, Password, CreatedOn, LastLogon)
                                        VALUES
                                            (@Name, @Email, @Password, @CreatedOn, @LastLogon);
                                        SELECT SCOPE_IDENTITY();";

                command.Parameters.AddWithValue("Name", user.Name);
                command.Parameters.AddWithValue("Email", user.Email);
                command.Parameters.AddWithValue("Password", user.Password);
                command.Parameters.AddWithValue("CreatedOn", user.CreatedOn);
                command.Parameters.AddWithValue("LastLogon", user.LastLogon);

                var commandResult = await command.ExecuteScalarAsync();

                user.Id = long.Parse(commandResult.ToString());

                connection.Close();
            }

            return user;
        }

        /// <inheritdoc />
        public async Task<User> UpdateUserAsync(User user)
        {
            if (user.Id == 0) throw new ArgumentException("User ID not informed!");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = @"UPDATE
                                            [dbo].[User]
                                        SET
                                            Name = @Name,
                                            Email = @Email,
                                            Password = @Password,
                                            UpdatedOn = @UpdatedOn
                                        WHERE Id = @Id";

                command.Parameters.AddWithValue("Name", user.Name);
                command.Parameters.AddWithValue("Email", user.Email);
                command.Parameters.AddWithValue("Password", user.Password);
                command.Parameters.AddWithValue("UpdatedOn", user.UpdatedOn);
                command.Parameters.AddWithValue("Id", user.Id);

                await command.ExecuteNonQueryAsync();

                connection.Close();
            }

            return user;
        }

        /// <inheritdoc />
        public async Task UpdateLastLogonAsync(long userId, DateTime lastLogon)
        {
            if (userId == 0) throw new ArgumentException("User ID not informed!");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "UPDATE [dbo].[User] SET LastLogon = @LastLogon WHERE Id = @Id";

                command.Parameters.AddWithValue("LastLogon", lastLogon);
                command.Parameters.AddWithValue("Id", userId);

                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
        }

        /// <inheritdoc />
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var result = default(User);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = @"SELECT 
                                            Id, Name, Email, Password, CreatedOn, UpdatedOn, LastLogon
                                        FROM [dbo].[User]
                                        WHERE
                                            Email = @Email";

                command.Parameters.AddWithValue("Email", email);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        result = new User()
                        {
                            Id = reader.GetFieldValue<long>(0),
                            Name = reader.GetFieldValue<string>(1),
                            Email = reader.GetFieldValue<string>(2),
                            Password = reader.GetFieldValue<string>(3),
                            CreatedOn = reader.GetFieldValue<DateTime>(4),
                            UpdatedOn = reader.IsDBNull(5) ? default(DateTime?) : reader.GetFieldValue<DateTime>(5),
                            LastLogon = reader.GetFieldValue<DateTime>(6)
                        };
                    }
                }

                connection.Close();
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<User> GetUserByIdAsync(long userId)
        {
            var result = default(User);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = @"SELECT 
                                            Id, Name, Email, Password, CreatedOn, UpdatedOn, LastLogon
                                        FROM [dbo].[User]
                                        WHERE
                                            Id = @Id";

                command.Parameters.AddWithValue("Id", userId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        result = new User()
                        {
                            Id = reader.GetFieldValue<long>(0),
                            Name = reader.GetFieldValue<string>(1),
                            Email = reader.GetFieldValue<string>(2),
                            Password = reader.GetFieldValue<string>(3),
                            CreatedOn = reader.GetFieldValue<DateTime>(4),
                            UpdatedOn = reader.IsDBNull(5) ? default(DateTime?) : reader.GetFieldValue<DateTime>(5),
                            LastLogon = reader.GetFieldValue<DateTime>(6)
                        };
                    }
                }

                connection.Close();
            }

            return result;
        }
    }
}