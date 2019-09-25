
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Demo.Models;
using Microsoft.Extensions.Configuration;

namespace Demo.Repository
{
    public interface IOrdersRepository
    {
        IEnumerable<Orders> GetAll();
        Orders FindById(int id);
        int Delete(int entity);
        int Update(Orders entity);
        int Add(Orders tModel);
        IEnumerable<Orders> FindByUserId(int id);
    }


    public class OrdersRepository : GenericRepository<Orders>, IOrdersRepository
    {
        private readonly IConfiguration _configuration;

        public OrdersRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public override string CreateSeleteString()
        {
            return "SELECT * FROM [Orders] ";
        }

        public override int Update(Orders tModel)
        {
            var sqlCommand = string.Format(@"UPDATE [Orders] SET [ProductId] = @ProductId ,[UserId] = @UserId where [Id] =@Id");
            return this.DbConnection.Execute(sqlCommand, new
            {
                tModel.Id,
                tModel.ProductId,
                tModel.UserId
            });
        }

        public override int Delete(int id)
        {
            var sqlCommand = @"DELETE FROM [Orders] WHERE [Id] = @Id";
            return DbConnection.Execute(sqlCommand, new
            {
                id
            });
        }

        public override int Add(Orders tModel)
        {
            var sqlCommand = @"INSERT INTO [Orders] ([ProductId],[UserId]) VALUES (@ProductId,@UserId)SELECT CAST(SCOPE_IDENTITY() as int)";

            var resut = DbConnection.ExecuteScalar<int>(sqlCommand, new
            {
                tModel.Id,
                tModel.ProductId,
                tModel.UserId
            });
            tModel.Id = resut;
            return resut;
        }

        public IEnumerable<Orders> FindByUserId(int id)
        {
            var sqlCommand = @"SELECT * FROM [Orders] WHERE [UserId] = @UserId";
            return this.DbConnection.Query<Orders>(sqlCommand, new
            {
                IdUser = id
            }).ToList();
        }
    }
}
