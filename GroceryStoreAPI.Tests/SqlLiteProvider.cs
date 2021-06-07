using GroceryStoreAPI.Data.DataProviders;
using GroceryStoreAPI.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace GroceryStoreAPI.Tests
{
    public class SqlLiteProvider
    {
        private const string InMemmoryConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;
        protected readonly GroceryStoreAPIDBContext DbContext;
        private bool _disposed = false;

        protected SqlLiteProvider()
        {
            _connection = new SqliteConnection(InMemmoryConnectionString);
            _connection.Open();
            var options = new DbContextOptionsBuilder<GroceryStoreAPIDBContext>()
                    .UseSqlite(_connection)
                    .Options;
            DbContext = new GroceryStoreAPIDBContext(options);
            DbContext.Database.EnsureCreated();
            void seedAction() => SeedData.SeedTypesOnly(DbContext);
            seedAction();

        }
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _connection.Close();
            }
            _disposed = true;
        }
    }

    public static class SeedData
    {
        public async static void SeedTypesOnly(GroceryStoreAPIDBContext dbContext)
        {
            var customer = new Customer
            {
                ID = 1,
                Name = "Test Customer"
            };

            dbContext.Customer.Add(customer);
            dbContext.SaveChanges(true);
        }
    }
}
