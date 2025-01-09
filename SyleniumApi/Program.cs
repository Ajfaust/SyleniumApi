using System.Reflection;
using System.Text.Json.Serialization;
using SyleniumApi.DbContexts;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SyleniumApi.Interfaces;
using SyleniumApi.Services;

var builder = WebApplication.CreateBuilder(args);
var myPolicy = "MyPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myPolicy,
                      policy =>
                      {
                          policy.AllowAnyOrigin();
                          policy.AllowAnyMethod();
                          policy.AllowAnyHeader();
                      });
});

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
// builder.Services.AddDbContext<SyleniumContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("SyleniumDB")));

builder.Services.AddScoped<IJournalService, JournalService>();
builder.Services.AddDbContext<SyleniumContext>(options => options.UseNpgsql("Host=192.168.39.15;Port=5432;Database=sylenium;Username=syleniumdev;Password=syldev"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(myPolicy);

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}