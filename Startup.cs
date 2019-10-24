using mdaWar.Helpers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;


[assembly: FunctionsStartup(typeof(mdaWar.Startup))]
namespace mdaWar
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<Context>(
                options => options.UseSqlServer(Environment.GetEnvironmentVariable("SqlConnectionString"), sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));


            builder.Services.AddSingleton(new Random());
            builder.Services.AddSingleton(typeof(BattleHelper));
            builder.Services.AddSingleton(typeof(WeaponsHelper));

        }
    }
}
