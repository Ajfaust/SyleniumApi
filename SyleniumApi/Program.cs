using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SyleniumApi.DbContexts;

var builder = WebApplication.CreateBuilder(args);
var myPolicy = "MyPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(myPolicy,
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
//     .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });
builder.Services.AddDbContext<SyleniumDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("SyleniumDB")));

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(doc =>
    {
        doc.DocumentSettings = s =>
        {
            s.Title = "Sylenium API";
            s.Version = "v1";
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(options =>
// {
//     var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//     options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseCors(myPolicy);

app.UseSerilogRequestLogging();

// app.MapControllers();

app.UseFastEndpoints(c => { c.Endpoints.RoutePrefix = "api"; })
    .UseSwaggerGen();

app.UseHttpsRedirection();

app.Run();

public partial class Program
{
}