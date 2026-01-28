using FluentValidation.AspNetCore;
using OrderService.BusinessLogicLayer;
using OrderService.BusinessLogicLayer.HttpClients;
using OrderService.DataAccessLayer;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddDataAccessLayer();
// builder.Services.AddBusinessLogicLayer();
// builder.Services.AddControllers();

// builder.Services.AddFluentValidationAutoValidation();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// builder.Services.AddCors(options => options.AddDefaultPolicy(builder => builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();
builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => options.AddDefaultPolicy(builder => builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader()));
builder.Services.AddHttpClient<UsersMicroserviceClient>(client =>
{
    client.BaseAddress = new Uri($"http://{Environment.GetEnvironmentVariable("UsersMicroserviceName")}:{Environment.GetEnvironmentVariable("UsersMicroservicePort")}");
});
var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();
app.UseCors();

app.MapSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
