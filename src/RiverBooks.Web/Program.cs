using FastEndpoints;
using FastEndpoints.Swagger;
using RiverBooks.Books;
using RiverBooks.Users;
using Serilog;

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);

// After the app is initialized we can now register Serilog
builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddFastEndpoints()
  .AddAuthorization()
  .SwaggerDocument()
  .AddAuthentication().AddJwtBearer();

// Add Module Services
builder.Services.AddBookModuleServices(builder.Configuration, logger);
builder.Services.AddUserModuleServices(builder.Configuration, logger);

var app = builder.Build();

app.UseAuthentication()
  .UseAuthorization();

app.UseFastEndpoints()
  .UseSwaggerGen();

app.Run();

public partial class Program {} // needed for tests
