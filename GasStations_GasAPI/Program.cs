using GasStations_GasAPI.Data;
using GasStations_GasAPI.Handlers;
using GasStations_GasAPI.Middleware;
using GasStations_GasAPI.Services.GasStationService;
using GasStations_GasAPI.Services.UserService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IGasStationService, GasStationService>();
builder.Services.AddScoped<IUserService, UserService>();


Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log/GasStationLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

builder.Services.AddControllers(option =>
{
    //option.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddAuthentication("BasicAuthentication")
//    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.MapWhen(context => context.Request.Path.Equals("/api/GasAPI"), app => app.UseMiddleware<BasicAuthHandler>("Test"));

app.UseMiddleware<BasicAuthHandler>("Test");

app.UseHttpsRedirection();

//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
