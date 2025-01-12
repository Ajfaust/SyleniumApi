using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SyleniumApi.DbContexts;
using Testcontainers.PostgreSql;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Tests;

public class IntegrationTestFactory(ILogger logger) : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithLogger()
        .Build();

    public SyleniumDbContext DbContext { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        DbContext = Services.CreateScope().ServiceProvider.GetRequiredService<SyleniumDbContext>();
    }

    public new async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveDbContext<SyleniumDbContext>();
            services.AddDbContext<SyleniumDbContext>(options =>
            {
                options.UseNpgsql(_container.GetConnectionString());
            });
            services.EnsureDbCreated<SyleniumDbContext>();
        });
    }
}

public static class ServiceCollectionExtensions
{
    public static void RemoveDbContext<T>(this IServiceCollection services) where T : DbContext
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<T>));
        if (descriptor != null)
            services.Remove(descriptor);
    }

    public static void EnsureDbCreated<T>(this IServiceCollection services) where T : DbContext
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<T>();
        context.Database.EnsureCreated();
    }
}