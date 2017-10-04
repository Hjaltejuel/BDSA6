using BDSA2017.Assignment06.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BDSA2017.Assignment05.Entities
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SlotCarContext>
    {
        public SlotCarContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"C:\Users\Michelle\source\repos\BDSA2017.Assignment05\BDSA2017.Assignment05\appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<SlotCarContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new SlotCarContext(builder.Options);
        }


        public SlotCarContext CreateDbContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<SlotCarContext>().UseSqlite(connection).Options;

            var context = new SlotCarContext(builder);
            context.Database.EnsureCreated();
            return context;

        }


    }
}