using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Marge.Core.Queries.Handlers;
using Marge.Core.Queries.Models;

namespace Marge.Core.Queries.Data
{
    public class PriceRepository : IPriceQuery, IPriceSaver
    {
        public Price RetrieveById(Guid id)
           => Exec(db => db.QueryFirstOrDefault<Price>("select * from Prices where id = @id", new { id }));

        public IEnumerable<Price> RetrieveAll()
            => Exec(db => db.Query<Price>("select * from Prices"));

        public void Create(Price price)
            => Exec(db => db.Execute("insert Prices(Id, Amount, Discount, Profit) values (@Id, @Amount, @Discount, @Profit)", price));

        public void Update(Price price)
            => Exec(db => db.Execute("update Prices set Amount = @Amount, Discount = @Discount, Profit = @Profit where id = @id", price));

        private static T Exec<T>(Func<IDbConnection, T> f)
        {
            using (var db = CreateConnection()) return f(db);
        }

        private static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["MargeDb"].ConnectionString);
        }
    }
}