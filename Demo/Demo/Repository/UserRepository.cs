

using System.Collections.Generic;
using Dapper;
using Demo.Models;
using Microsoft.Extensions.Configuration;

namespace Demo.Repository
{
    public interface IUserRepository
    {
        int Add(Users tModel);
        int Delete(int id);
        IEnumerable<Users> GetAll();
        Users FindById(int id);
        int Update(Users tUsers);
        Users Login(string userName, string password);
        int ChangePass(string pass, int id);
    }
    public class UserRepository : GenericRepository<Users>, IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration) : base(configuration)
        {
            {
                _configuration = configuration;
            }
        }

        public override string CreateSeleteString()
        {
            return "SELECT * FROM [Users] ";
        }

        public override int Update(Users tModel)
        {
            var sqlCommand = @"UPDATE [Users] SET [UserName] = @UserName 
                        ,[FirstName] = @FirstName ,[LastName] =@LastName ,[Email] =@Email ,[Tel] =@Tel,[Password] = @Password WHERE [Id] = @Id";
            return DbConnection.Execute(sqlCommand, new
            {
                tModel.Id,
                tModel.UserName,
                tModel.FirstName,
                tModel.LastName,
                tModel.Email,
                tModel.Tel,
                tModel.Password
            });
        }

        public int ChangePass(string pass,int id)
        {
            var sqlCommand = @"UPDATE [Users] SET[Password] = @pass WHERE [Id] = @Id";
            return DbConnection.Execute(sqlCommand, new
            {
                id,
                pass 
            });
        }


        public override int Delete(int id)
        {
            var sqlCommand = @"DELETE FROM [Users] WHERE [Id] = @Id";
            return DbConnection.Execute(sqlCommand, new
            {
                id
            });
        }

        public Users Login(string userName, string password)
        {
            var sqlCommand = @"select * FROM [Users] WHERE [UserName] = @UserName and [Password ] = @Password";
            var result = DbConnection.QueryFirstOrDefault<Users>(sqlCommand, new
            {
                userName,
                password
            });

            return result ?? new Users();
        }

        public override int Add(Users tModel)
        {
            var sqlCommand = @"INSERT INTO [Users] ([UserName],[FirstName],[LastName],[Email],[Tel],[Password])
                            VALUES (@UserName,@FirstName,@LastName,@Email,@Tel,@Password)SELECT CAST(SCOPE_IDENTITY() as int)";

            var result = DbConnection.ExecuteScalar<int>(sqlCommand, new
            {
                tModel.Email,
                 tModel.FirstName,
                tModel.UserName,
                tModel.LastName,
                tModel.Tel,
                tModel.Password
            });
            tModel.Id = result;
            return result;
        }
    }
}
