using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EasyCare.Core.Infrastructure
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            //var connectionString = "Data Source=NS42\\SQLEXPRESS;Initial Catalog=EasyCareTestDB;User ID=mitul17;Password=mitul@17";
            var connectionString = "Server=tcp:mysqlserver0622.database.windows.net,1433;Initial Catalog=EasyCareDatabase;Persist Security Info=False;User ID=azureSQLadmin;Password=6B7Wrl0xc4Dt;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            builder.UseSqlServer(connectionString);
            return new DatabaseContext(builder.Options);
        }
    }
}
