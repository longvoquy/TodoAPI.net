using Microsoft.EntityFrameworkCore;
using TodoAppApi1.Data;
using System.Text.Json.Serialization;
using TodoAppApi1.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Thêm tất cả services TRƯỚC KHI builder.Build()
builder.Services.AddTransient<TodoListController>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddDbContext<ApiContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging();
});

// Thêm CORS ở đây (trước Build())
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
// Cho phép phục vụ file tĩnh (wwwroot)

// Build ứng dụng
var app = builder.Build();

// Middleware pipeline
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Route mặc định trả về file index.html
app.MapFallbackToFile("index.html");
// Sử dụng CORS (sau Build() nhưng trước Run())
app.UseCors("AllowAll");

app.Run("http://localhost:4000");