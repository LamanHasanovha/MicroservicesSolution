using Consul;
using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess;
using OrderService.Services.Abstract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LamanDbContext>(options =>
                             options.UseSqlServer(builder.Configuration.GetConnectionString("LamanDb")));

builder.Services.AddScoped<IOrderService, OrderService.Services.Concrete.OrderService>();

var app = builder.Build();

var consulClient = new ConsulClient(config => { config.Address = new Uri("http://localhost:8500"); });

var registration = new AgentServiceRegistration()
{
    ID = "order-service",
    Name = "order-service",
    Address = "localhost",
    Port = 5001
};

consulClient.Agent.ServiceRegister(registration).Wait();
app.MapGet("/", () => "OrderService is running...");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
