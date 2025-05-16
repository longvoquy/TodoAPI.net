using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TodoAppApi1.AppHost.Controller;
using MediatR;
using TodoAppApi1.Application.Common.Interface;
using TodoAppApi1.Application.TodoItems.Commands.CreateTodoItem;
using TodoAppApi1.Infrastructure.Persistence;
using TodoAppApi1.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = null // Tắt wwwroot
});

// 1. Đọc connection string theo thứ tự: appsettings.json -> biến môi trường
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Nếu không có trong appsettings.json, thử lấy từ biến môi trường DB_CONNECTION_STRING
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
}

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Database connection string not found in configuration or environment variables.");
}

Console.WriteLine($"ConnectionString: {connectionString}");

// Add services to the container.
builder.Services.AddTransient<TodoListController>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Cấu hình DbContext với connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    options.EnableSensitiveDataLogging(); // chỉ bật khi dev
});

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());

// Đăng ký MediatR (tất cả handlers trong assembly của CreateTodoItemCommand)
builder.Services.AddMediatR(typeof(CreateTodoItemCommand).Assembly);

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseRouting();

app.UseCors("AllowAll");

app.MapControllers();

// Tạo database khi chạy (nếu chưa có)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();
