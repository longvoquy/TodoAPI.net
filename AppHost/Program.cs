using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TodoAppApi1.AppHost.Controller;
using MediatR;
using TodoAppApi1.Application.Common.Interface;
using TodoAppApi1.Application.TodoItems.Commands.CreateTodoItem;
using TodoAppApi1.Application.TodoItems.Commands.DeleteTodoItem;
using TodoAppApi1.Application.TodoItems.Commands.UpdateTodoItem;
using TodoAppApi1.Application.TodoLists.Commands.CreateTodoList;
using TodoAppApi1.Application.TodoLists.Commands.DeleteTodoList;
using TodoAppApi1.Application.TodoLists.Commands.UpdateTodoList;
using TodoAppApi1.Infrastructure.Persistence;
using TodoAppApi1.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = null // Tắt wwwroot
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"ConnectionString: {connectionString}");

// Add services to the container.
builder.Services.AddTransient<TodoListController>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connString))
    {
        throw new InvalidOperationException("Database connection string not found");
    }
    options.UseNpgsql(connString);
    options.EnableSensitiveDataLogging();
});

builder.Services.AddScoped<IApplicationDbContext>(provider => 
    provider.GetRequiredService<ApplicationDbContext>());

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreateTodoItemCommand>();
    cfg.RegisterServicesFromAssemblyContaining<DeleteTodoItemCommand>();
    cfg.RegisterServicesFromAssemblyContaining<UpdateTodoItemCommand>();
    cfg.RegisterServicesFromAssemblyContaining<CreateTodoListCommand>();
    cfg.RegisterServicesFromAssemblyContaining<UpdateTodoListCommand>();
    cfg.RegisterServicesFromAssemblyContaining<DeleteTodoListCommand>();
    
});
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
// Add CORS
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
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}
app.Run();