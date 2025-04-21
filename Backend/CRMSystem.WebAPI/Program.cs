using CRMSystem.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Serilog
builder.AddSerilogLogging();

// Connection to Database
builder.Services.AddDbConnection(configuration);

// Adding Services
builder.Services.AddApplicationServices();
builder.Services.AddAuthenticationServices(configuration);
builder.Services.AddSmtpConfiguration(configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Authorization Policy
builder.Services.AddAuthorizationPolicy();

var app = builder.Build();

// DB Initialization
app.AddDbInitialization();

// Using Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Cookies Policy
app.AddCookiePolicy();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();