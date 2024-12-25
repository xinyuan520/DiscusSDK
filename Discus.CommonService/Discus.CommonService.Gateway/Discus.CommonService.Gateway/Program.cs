using Discus.SDK.Core.Configuration;
using Discus.SDK.Core.Microsoft.Extensions.Hosting;
using Discus.SDK.Nacos.Extensions;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Nacos;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile($"{AppContext.BaseDirectory}/ocelot.json", true, true);
builder.Configuration.AddJsonFile($"{AppContext.BaseDirectory}/hosting.json", true, true);

#region 获取appsetting.json配置文件
var tokenConfig = builder.Configuration.GetSection("JWT").Get<JWTConfig>();
var threadPoolConfig = builder.Configuration.GetSection("ThreadPoolSettings");
var nacosOption = builder.Configuration.GetSection("nacos").Get<NacosConfig>();
#endregion
builder.Configuration.AddNacosConfiguration(nacosOption, true);

builder.Services.Configure<ThreadPoolSettings>(threadPoolConfig)
    .AddAuthentication()
    .AddJwtBearer("mgmt", options =>
    {
        var tokenConfig = builder.Configuration.GetSection("JWT").Get<JWTConfig>();
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = tokenConfig.ValidateIssuer,
            ValidIssuer = tokenConfig.ValidIssuer,
            ValidateIssuerSigningKey = tokenConfig.ValidateIssuerSigningKey,
            IssuerSigningKey = new SymmetricSecurityKey(tokenConfig.Encoding.GetBytes(tokenConfig.SymmetricSecurityKey)),
            ValidateAudience = tokenConfig.ValidateAudience,
            ValidAudience = tokenConfig.ValidAudience,
            ValidateLifetime = tokenConfig.ValidateLifetime,
            RequireExpirationTime = tokenConfig.RequireExpirationTime,
            ClockSkew = TimeSpan.FromSeconds(tokenConfig.ClockSkew),
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("default", policy =>
    {
        var corsHosts = builder.Configuration.GetValue<string>("CorsHosts");
        var corsHostsArray = corsHosts.Split(',');
        policy.WithOrigins(corsHostsArray)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
})
    .AddHttpLogging(logging =>
    {
        logging.LoggingFields = HttpLoggingFields.All;
        logging.RequestBodyLogLimit = 4096;
        logging.ResponseBodyLogLimit = 4096;
    })
    .AddOcelot(builder.Configuration)
    .AddNacosDiscovery()
    .AddPolly();

var app = builder.Build();

app.UseCors("default")
    .UseHttpLogging()
    .UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapGet("/", async context =>
        {
            await context.Response.WriteAsync($"Hello Ocelot!");
        });
    })
    .UseOcelot().Wait();

app.Run();