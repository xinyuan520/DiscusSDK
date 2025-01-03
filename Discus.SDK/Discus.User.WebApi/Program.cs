using Discus.User.Application.Registrar;
using SkyApm.Utilities.DependencyInjection;

var webApiAssembly = System.Reflection.Assembly.GetExecutingAssembly();
var applicationFullName = UserApplicationDependencyRegistrar.ApplicationFullName;
var serviceInfo = ServiceInfo.CreateInstance(webApiAssembly, applicationFullName);
Environment.SetEnvironmentVariable("ASPNETCORE_HOSTINGSTARTUPASSEMBLIES", "SkyAPM.Agent.AspNetCore");
Environment.SetEnvironmentVariable("SKYWALKING__SERVICENAME", serviceInfo.ServiceName);

var builder = WebApplication.CreateBuilder(args);
var app = builder.ConfigureDefault(serviceInfo).Build();
app.UseCustomMiddleware();

await app.ChangeThreadPoolSettings().RunAsync();
