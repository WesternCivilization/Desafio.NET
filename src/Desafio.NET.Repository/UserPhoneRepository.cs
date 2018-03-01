using Dapper;
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
    /// Implementation for userPhone repository contracts
    /// </summary>
    public class UserPhoneRepository : IUserPhoneRepository
    {
        private readonly string _connectionString;

        /// <inheritdoc />
        public UserPhoneRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <inheritdoc />
        public async Task<UserPhone> CreateUserPhoneAsync(UserPhone userPhone)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = @"INSERT INTO [dbo].[UserPhone] 
                                            (UserId, AreaCode, PhoneNumber)
                                        VALUES
                                            (@UserId, @AreaCode, @PhoneNumber)
                                        SELECT SCOPE_IDENTITY();";

                command.Parameters.AddWithValue("UserId", userPhone.UserId);
                command.Parameters.AddWithValue("AreaCode", userPhone.AreaCode);
                command.Parameters.AddWithValue("PhoneNumber", userPhone.PhoneNumber);

                var commandResult = await command.ExecuteScalarAsync();

                userPhone.Id = long.Parse(commandResult.ToString());

                connection.Close();
            }

            return userPhone;
        }

        /// <inheritdoc />
        public async Task DeleteUserPhonesAsync(long userId)
        {
            if (userId == 0) throw new ArgumentException("User ID not informed!");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = @"DELETE FROM [dbo].[UserPhone] WHERE UserId = @UserId";

                command.Parameters.AddWithValue("UserId", userId);

                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
        }

        /// <inheritdoc />
        public async Task<UserPhone[]> GetUserPhonesAsync(long userId)
        {
            IEnumerable<UserPhone> result = new List<UserPhone>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var commandText = @"SELECT Id, UserId, AreaCode, PhoneNumber
                                        FROM
                                    [dbo].[UserPhone]
                                        WHERE UserId = @UserId";

                result = await connection.QueryAsync<UserPhone>(commandText, new { UserId = userId });

                connection.Close();
            }

            return result.ToArray();
        }
    }
}