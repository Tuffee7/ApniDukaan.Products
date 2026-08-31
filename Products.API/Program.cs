using FluentValidation.AspNetCore;
using Products.API.APIEndpoints;
using Products.API.Middleware;
using Products.Business;
using Products.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Adding Data and Business Services to the IoC container
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();

// Add services to the container.
builder.Services.AddControllers();

// Adding Fluent Validation
builder.Services.AddFluentValidationAutoValidation();

// Add model binder to read the request body as JSON and bind it to the model
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(x =>
    {
        x.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();

// Cors
app.UseCors();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Auth
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapProductAPIEndPoints();

app.Run();
