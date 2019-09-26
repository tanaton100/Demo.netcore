using System.Collections.Generic;
using Dapper;
using Demo.Models;
using Microsoft.Extensions.Configuration;

namespace Demo.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Products> GetAll();
        Products FindById(int id);
        int Add(Products entity);
        int Update(Products entity);
        int Delete(int id);
    }
    public class ProductRepository : GenericRepository<Products>, IProductRepository
    {
        private readonly IConfiguration _configuration;

        public ProductRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public override string CreateSeleteString()
        {
            return "SELECT * FROM [Products] ";

        }

        public override int Update(Products tModel)
        {
            const string sqlCommand = @"INSERT INTO [Products] ([Name],[Price]) VALUES (@Name,@Price)SELECT CAST(SCOPE_IDENTITY() as int)";
            return DbConnection.ExecuteScalar<int>(sqlCommand, new
            {
                tModel.Id,
                tModel.Name,
                tModel.Price
            });
        }

        public override int Delete(int id)
        {
            var sqlCommand = @"DELETE FROM [Products] WHERE [Id] = @Id";
            return DbConnection.Execute(sqlCommand, new
            {
                id
            });
        }

        public override int Add(Products tModel)
        {
            const string sqlCommand = @"INSERT INTO [Products] ([Name],[Price]) VALUES (@Name,@Price)SELECT CAST(SCOPE_IDENTITY() as int)";
            return DbConnection.ExecuteScalar<int>(sqlCommand, new
            {
                tModel.Name,
                tModel.Price
            });
        }
    }
}
