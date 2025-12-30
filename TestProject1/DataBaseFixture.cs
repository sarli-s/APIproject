using Castle.Core.Configuration;
using Entitys;
using Microsoft.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestProject
{
    public class DatabaseFixture : IDisposable
    {
        public dbSHOPContext Context { get; private set; }

        public DatabaseFixture()
        {
            
            // Set up the test database connection and initialize the context
            var options = new DbContextOptionsBuilder<dbSHOPContext>()
                .UseSqlServer("Server=DESKTOP-ER24GCJ\\SQLEXPRESS;Database=215949413_SHOP;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;
            Context = new dbSHOPContext(options);
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            // Clean up the test database after all tests are completed
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
