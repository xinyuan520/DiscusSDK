using DotXxlJob.Core;
using JobWebApiDemo;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"{AppContext.BaseDirectory}/hosting.json", true, true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddXxlJobExecutor(builder.Configuration);
builder.Services.AddDefaultXxlJobHandlers();


// 添加日志记录
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.AddSingleton<IJobHandler, DiscusJobHandler>();

builder.Services.AddAutoRegistry();

var app = builder.Build();

app.UseMiddleware<XxlJobExecutorMiddleware>();

app.UseAuthorization();

app.MapControllers();


app.Run();
