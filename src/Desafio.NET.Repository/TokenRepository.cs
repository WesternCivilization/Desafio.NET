using Desafio.NET.Repository.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Desafio.NET.Domain.Entities;
using System.Data.SqlClient;
using Dapper;

namespace Desafio.NET.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly string _connectionString;

        /// <inheritdoc />
        public TokenRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <inheritdoc />
        public async Task<Token> CreateTokenAsync(Token token)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var commandText = @"INSERT INTO [dbo].[Token] 
                                            (UserId, Code, IssuedOn, ExpireOn)
                                        VALUES
                                            (@UserId, @Code, @IssuedOn, @ExpireOn)
                                        SELECT SCOPE_IDENTITY();";

                token.Id = await connection.ExecuteScalarAsync<long>(commandText, token);

                connection.Close();
            }

            return token;
        }

        /// <inheritdoc />
        public async Task DeleteUserTokensAsync(long userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var commandText = @"DELETE FROM [dbo].[Token] WHERE UserId = @UserId";

                await connection.ExecuteAsync(commandText, new { UserId = userId });

                connection.Close();
            }
        }

        /// <inheritdoc />
        public async Task<Token> GetTokenByCodeAsync(string code)
        {
            var result = default(Token);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var commandText = @"SELECT Id, UserId, Code, IssuedOn, ExpireOn FROM [dbo].[Token] WHERE Code = @Code;";

                result = await connection.QuerySingleOrDefaultAsync<Token>(commandText, new { Code = code });

                connection.Close();
            }

            return result;
        }
    }
}