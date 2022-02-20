using System.Reflection;
using MediatR;
using PuppyPlace.CqrsService;
using PuppyPlace.Data;
using PuppyPlace.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PuppyPlaceContext>();
builder.Services.AddTransient<IDogRepository, DogRepository>();
builder.Services.AddTransient<IPersonRepository, PersonRepository>();
builder.Services.AddMediatR(typeof(MediatorEntry).Assembly);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.MapControllers();

app.Run();