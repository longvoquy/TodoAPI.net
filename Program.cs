using Microsoft.EntityFrameworkCore;
using TodoAppApi1.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);//create builder (setupDI & app function)
//add service to the container
builder.Services.AddDbContext<ApiContext>
(opt=> opt.UseInMemoryDatabase("TodoAppApi"));//setting name for todoapp database
builder.Services.AddControllers() // controller enum -> string 
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

//configuring Swagger/OpenApi
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
//build app, run pipeline
var app = builder.Build();
//run swagger as dev mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//middleware
app.UseHttpsRedirection(); // Redirect HTTP sang HTTPS

app.UseAuthorization();// Dùng nếu có gắn [Authorize] vào Controller

app.MapControllers();// Map các Controller đã tạo để xử lý route

app.Run();