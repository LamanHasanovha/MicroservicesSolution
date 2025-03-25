using Consul;
using Microsoft.EntityFrameworkCore;
using ProductService.DataAccess;
using ProductService.Services.Abstract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LamanDbContext>(options => 
                             options.UseSqlServer(builder.Configuration.GetConnectionString("LamanDb")));

builder.Services.AddScoped<IProductService, ProductService.Services.Concrete.ProductService>();

var app = builder.Build();

var consulClient = new ConsulClient(config => { config.Address = new Uri("http://localhost:8500"); });

var registration = new AgentServiceRegistration()
{
    ID = "product-service",
    Name = "product-service",
    Address = "localhost",
    Port = 5001
};

consulClient.Agent.ServiceRegister(registration).Wait();
app.MapGet("/", () => "ProductService is running...");

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
