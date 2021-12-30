using System.Text.Json.Serialization;
using PuppyPlace.Data;
using PuppyPlace.Domain;
using PuppyPlace.Repository;
using PuppyPlace.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddTransient<PersonService>();
builder.Services.AddTransient<PersonValidator>();
builder.Services.AddTransient<DogRepository>();
builder.Services.AddTransient<IPersonRepository, PersonRepository>();
builder.Services.AddDbContext<PuppyPlaceContext>();
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