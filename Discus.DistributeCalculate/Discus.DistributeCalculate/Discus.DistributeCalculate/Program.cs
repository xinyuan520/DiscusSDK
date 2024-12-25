using Discus.DistributeCalculate;
using Discus.DistributeCalculate.Handler;
using DotXxlJob.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile($"{AppContext.BaseDirectory}/hosting.json", true, true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddXxlJobExecutor(builder.Configuration);
builder.Services.AddDefaultXxlJobHandlers();

// 添加日志记录
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.AddSingleton<IJobHandler, DiscusJobHandler>();

builder.Services.AddAutoRegistry();

var app = builder.Build();

app.UseMiddleware<XxlJobExecutorMiddleware>();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
