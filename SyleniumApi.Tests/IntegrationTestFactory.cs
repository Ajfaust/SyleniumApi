using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using Testcontainers.PostgreSql;

namespace SyleniumApi.Tests;

public static class DefaultTestValues
{
    public const int Id = 1;
}

public class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
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
                options.UseNpgsql(_container.GetConnectionString() + ";Include Error Detail=true");
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
        if (context is SyleniumDbContext syleniumDbContext)
            SeedDatabase(syleniumDbContext);
    }

    private static void SeedDatabase(SyleniumDbContext context)
    {
        // Add one of each entity for foreign key constraints
        context.Ledgers.Add(new Ledger
        {
            Name = "Test Ledger",
            CreatedDate = DateTime.UtcNow
        });
        context.SaveChanges();

        context.FinancialAccountCategories.Add(new FinancialAccountCategory
        {
            LedgerId = DefaultTestValues.Id,
            Type = FinancialCategoryType.Asset,
            Name = "Test Category"
        });
        context.SaveChanges();

        context.FinancialAccounts.Add(new FinancialAccount
        {
            LedgerId = DefaultTestValues.Id,
            FinancialAccountCategoryId = DefaultTestValues.Id,
            Name = "Test Account"
        });
        context.SaveChanges();

        context.Vendors.Add(new Vendor
        {
            LedgerId = DefaultTestValues.Id,
            Name = "Test Vendor"
        });
        context.SaveChanges();

        context.TransactionCategories.Add(new TransactionCategory
        {
            LedgerId = DefaultTestValues.Id,
            ParentCategoryId = null,
            Name = "Test Category"
        });
        context.SaveChanges();
    }
}