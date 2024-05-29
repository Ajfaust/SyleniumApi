using AllostaServer.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using Testcontainers.PostgreSql;

namespace AllostaServer.Test
{
    public class AllostaServerTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _container;

        public AllostaServerTestFactory()
        {
            _container = new PostgreSqlBuilder()
                .WithDatabase("budgetdb_test")
                .WithImage("postgres:16")
                .WithCleanUp(true)
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveDbContext<BudgetContext>();

                services.AddDbContext<BudgetContext>(options => options.UseNpgsql(_container.GetConnectionString()));

                services.EnsureDbCreated<BudgetContext>();
            });
        }

        public async Task InitializeAsync()
        {
            await _container.StartAsync();
            //using var scope = Services.CreateScope();
            //var dbContext = scope.ServiceProvider.GetRequiredService<BudgetContext>();
            //await dbContext.Database.MigrateAsync();
        }

        public new async Task DisposeAsync() => await _container.DisposeAsync();
    }
}