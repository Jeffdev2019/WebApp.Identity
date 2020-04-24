﻿using Dapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp.Identity
{
    public class MyUserStore : IUserStore<MyUser>, IUserPasswordStore<MyUser> 
    {
        public static DbConnection GetDbConnection()
        {
            var connection = new SqlConnection("Integrated Security=SSPI;" +
                "Persist Security Info=False;" +
                "Initial Catalog=IdentityCurso;" +
                @"Data Source=DESKTOP-4CLBSVQ\MSSQLSERVER_2");
            return connection;
        }

        public async Task<IdentityResult> CreateAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetDbConnection())
            {
                await connection.ExecuteAsync(
                    "INSERT INTO USERS([ID]," +
                    "[USERNAME]," +
                    "[NORMALIZEDUSERNAME]," +
                    "[PASSWORDHASH]) " +
                    "VALUES(@id, @userName, @normalizedUserName, @passwordHash)",
                    new
                    {
                        id = user.Id,
                        userName = user.UserName,
                        normalizedUserName = user.NormalizedUserName,
                        passwordHash = user.PasswordHash
                    });
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetDbConnection())
            {
                await connection.ExecuteAsync("DELETE FROM USERS WHERE [ID] = @id",
                    new
                    {
                        id = user.Id
                    });
            }
            return IdentityResult.Success;
        }

        public void Dispose()
        {

        }

        public async Task<MyUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var connection = GetDbConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<MyUser>(
                    "SELECT * FROM USERS WHERE [ID] = @id",
                    new { id = userId});
            };
        }

        public async Task<MyUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var connection = GetDbConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<MyUser>(
                    "SELECT * FROM USERS WHERE [NORMALIZEDUSERNAME] = @nome",
                    new { nome = normalizedUserName });
            };
        }

        public Task<string> GetNormalizedUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetUserIdAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName != null);
        }

        public Task SetNormalizedUserNameAsync(MyUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(MyUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(MyUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetDbConnection())
            {
                await connection.ExecuteAsync(
                    "UPDATE USERS" +
                    "SET [ID] = @id," +
                    "[USERNAME] = @userName," +
                    "[NORMALIZEDUSERNAME] = @normalizedUserName," +
                    "[PASSWORDHASH] = @passwordHash" +
                    "WHERE [ID] = @id",
                    new 
                    {
                        id = user.Id,
                        userName = user.UserName,
                        normalizedUserName = user.NormalizedUserName,
                        passwordHash = user.PasswordHash
                    });
            }
            return IdentityResult.Success;
        }
    }
}
