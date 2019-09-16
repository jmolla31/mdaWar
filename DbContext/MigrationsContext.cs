using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace mdaWar
{
    public class MigrationsContext : IDesignTimeDbContextFactory<Context>
    {
        private const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=mdaWar_temp;Integrated Security=True;";

        public Context CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<Context>();
            builder.UseSqlServer(ConnectionString);
            return new Context(builder.Options);
        }
    }
}
