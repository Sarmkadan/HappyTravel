using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HappyTravel.VaultClient;
using HappyTravel.VaultClient.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HappyTravel.Edo.Data
{
    public class EdoContextFactory : IDesignTimeDbContextFactory<EdoContext>
    {
        public EdoContext CreateDbContext(string[] args)
        {
            var dbContextOptions = new DbContextOptionsBuilder<EdoContext>();
            dbContextOptions.UseNpgsql(GetConnectionString(), builder => builder.UseNetTopologySuite());
            return new EdoContext(dbContextOptions.Options);
        }


        private static string GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("contextSettings.json", false, true)
                .Build();

            var dbOptions = GetDbOptions(configuration);
            
            return string.Format(ConnectionStringTemplate,
                dbOptions["host"],
                dbOptions["port"],
                dbOptions["userId"],
                dbOptions["password"]);
        }


        private static Dictionary<string, string> GetDbOptions(IConfiguration configuration)
        {
            using (var vaultClient = CreateVaultClient(configuration))
            {
                vaultClient.Login(Environment.GetEnvironmentVariable(configuration["Vault:Token"])).Wait();
                return vaultClient.Get(configuration["EDO:Database:Options"]).Result;
            }
        }


        private static IVaultClient CreateVaultClient(IConfiguration configuration)
        {
            var services = new ServiceCollection();
            services.AddVaultClient(o =>
            {
                o.Engine = configuration["Vault:Engine"];
                o.Role = configuration["Vault:Role"];
                o.Url = new Uri(Environment.GetEnvironmentVariable(configuration["Vault:Endpoint"]), UriKind.Absolute);
            });
            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetRequiredService<IVaultClient>();
        }


        private const string ConnectionStringTemplate = "Server={0};Port={1};Database=edo;Userid={2};Password={3};";
    }
}