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

var consulConfig = builder.Configuration.GetSection("ConsulConfig");
var consulClient = new ConsulClient(config =>
{
    config.Address = new Uri(consulConfig["Host"]);
});

var registration = new AgentServiceRegistration
{
    ID = consulConfig["ServiceId"],
    Name = consulConfig["ServiceName"],
    Address = consulConfig["ServiceAddress"],
    Port = int.Parse(consulConfig["ServicePort"]),
    Check = new AgentServiceCheck
    {
        HTTP = $"{consulConfig["ServiceAddress"]}:{consulConfig["ServicePort"]}/health",
        Interval = TimeSpan.FromSeconds(10),
        Timeout = TimeSpan.FromSeconds(5)
    }
};
await consulClient.Agent.ServiceDeregister(registration.ID);
await consulClient.Agent.ServiceRegister(registration);

app.MapGet("/health", () => Results.Ok("Healthy"));

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
