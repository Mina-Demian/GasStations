using GasStations_GasAPI.Data;
using GasStations_GasAPI.Handlers;
using GasStations_GasAPI.JWTAuthorization;
using GasStations_GasAPI.Middleware;
using GasStations_GasAPI.Models;
using GasStations_GasAPI.Models.API_Key.Service;
using GasStations_GasAPI.Models.API_Key;
using GasStations_GasAPI.Services.GasStationService;
using GasStations_GasAPI.Services.UserService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;
using System.Text;
using GasStations_GasAPI.ApiKey;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null)
//    .AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//    };
//});


// Add services to the container.

builder.Services.AddScoped<IGasStationService, GasStationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();


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



//builder.Services.AddSingleton<ApiKeyAuthorizationFilter>();

//builder.Services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();



var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

};

builder.Services.AddSingleton(tokenValidationParameters);

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>();

builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null)
    //.AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationSchemeHandler>("ApiKey", options => options.ApiKey = builder.Configuration["ApiKey"])
    .AddJwtBearer("Jwt", options =>
{
    options.TokenValidationParameters = tokenValidationParameters;
    //new TokenValidationParameters
    //{
    //    ValidateIssuer = true,
    //    ValidateAudience = true,
    //    ValidateLifetime = true,
    //    ValidateIssuerSigningKey = true,
    //    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    //    ValidAudience = builder.Configuration["Jwt:Audience"],
    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

    //};
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Level1", policy =>
    {
        policy.AuthenticationSchemes.Add("Jwt");
        //policy.AuthenticationSchemes.Add("BasicAuthentication");
        //policy.AuthenticationSchemes.Add("BasicAuthentication");
        policy.RequireClaim(ClaimTypes.Role, "Employee", "Admin", "Manager");
    });

    options.AddPolicy("Level2", policy =>
    {
        policy.AuthenticationSchemes.Add("Jwt");
        policy.RequireClaim(ClaimTypes.Role, "Admin", "Manager");
    });

    options.AddPolicy("Level3", policy =>
    {
        policy.AuthenticationSchemes.Add("Jwt");
        policy.RequireClaim(ClaimTypes.Role, "Manager");
    });
}
);



//builder.Services.AddAuthentication("BasicAuthentication")
//    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);


//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//    };
//});



//builder.Services.AddAuthorization(options =>
//{
//    // authorize using custom auth scheme only
//    options.AddPolicy("Basic Authentication", policy =>
//    {
//        policy.AuthenticationSchemes.Add("BasicAuthentication");
//        policy.RequireUserName("Mina");
//    });

//    // authorize using custom auth scheme as well as identity server
//    options.AddPolicy("JWT Bearer Authentication", policy =>
//    {
//        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
//        policy.RequireRole("Admin");
//    });
//});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.MapWhen(context => context.Request.Path.Equals("/api/GasAPI"), app => app.UseMiddleware<BasicAuthHandler>("Test"));

//app.UseMiddleware<BasicAuthHandler>("Test");

//app.UseMiddleware<JwtMiddleware>();

//app.MapWhen(context => context.Request.Path.Equals("/api/GasAPI/{id}"), app => app.UseMiddleware<JwtMiddleware>());

//app.UseMiddleware<JwtAuthentication>();

app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
