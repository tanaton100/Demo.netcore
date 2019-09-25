
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Demo.Repository
{
    public abstract class GenericRepository <TModel>
    {
        public IDbConnection DbConnection { get; set; }

        protected GenericRepository(IConfiguration configuration)
        {
            var interconnecting = configuration.GetSection("MsSqlConnectionString");
            DbConnection = new SqlConnection(interconnecting.Value);
        }

        public IEnumerable<TModel> GetAll()
        {

            return DbConnection.Query<TModel>(CreateSeleteString()).ToList();
        }

        public TModel FindById(int id)
        {
            return DbConnection.Query<TModel>(CreateSeleteString() + " WHERE Id = @Id", new
            {
                id
            }).FirstOrDefault();
        }

        public abstract string CreateSeleteString();
        public abstract int Update(TModel tModel);
        public abstract int Delete(int id);
        public abstract int Add(TModel tModel);
    }
}
