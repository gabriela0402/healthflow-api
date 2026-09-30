using HealthFlow.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using HealthFlow.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using HealthFlow.Repository.Interfaces;
using HealthFlow.Repository.Repositories;
using HealthFlow.Service.Interfaces;
using HealthFlow.Service.Services;
using HealthFlow.Api.Middlewares;
using HealthFlow.Repository.Interfaces;
using HealthFlow.Repository.Repositories;
using HealthFlow.Service.Interfaces;
using HealthFlow.Service.Services;



var builder = WebApplication.CreateBuilder(args);


var connectionString =
    builder.Configuration.GetConnectionString("HealthFlowConnection")
    ?? throw new InvalidOperationException(
        "A connection string 'HealthFlowConnection' não foi encontrada.");

builder.Services.AddDbContext<HealthFlowDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<ISpecialtyRepository,SpecialtyRepository>();
builder.Services.AddScoped<ISpecialtyService,SpecialtyService>();



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();


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
