using System.Reflection;
using Carter;
using FluentValidation;
using SyleniumApi.DbContexts;
using Microsoft.EntityFrameworkCore;
using Serilog;

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

// builder.Services.AddControllers()
//     .AddJsonOptions(options =>
//     {
//         options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
//     });
// builder.Services.AddDbContext<SyleniumContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("SyleniumDB")));

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(assembly)
);

builder.Services.AddCarter();

builder.Services.AddValidatorsFromAssembly(assembly);

// builder.Services.AddScoped<IJournalService, JournalService>();
builder.Services.AddDbContext<SyleniumDbContext>(options => options.UseNpgsql("Host=localhost;Port=8432;Database=syleniumdev;Username=syleniumdev;Password=syldev"));

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

app.MapCarter();

app.UseHttpsRedirection();

app.Run();

public partial class Program
{
}