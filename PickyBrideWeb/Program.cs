using HostedServiceAndDI.Entities;
using HostedServiceAndDI.Repositories;
using SecretaryProblem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EnvironmentContext>();
builder.Services.AddSingleton<Hall>();
builder.Services.AddSingleton<Friend>();
builder.Services.AddScoped<ContenderRepository>();

//builder.Services.AddMassTransit()

var app = builder.Build();
app.UseCors(b => b.AllowAnyOrigin());

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