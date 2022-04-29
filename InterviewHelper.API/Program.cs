using System.Text;
using InterviewHelper.Core.Helper;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.DataAccess.Repositories;
using InterviewHelper.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:3000"); // frontend origin
        });
}); // cors enabling

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)    
    .AddJwtBearer(options =>    
    {    
        options.TokenValidationParameters = new TokenValidationParameters    
        {    
            ValidateIssuer = false,    
            ValidateAudience = false,    
            ValidateLifetime = true,  
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthenticationSecret:Secret"]))    
        };    
    });    

// Add services to the container.
builder.Services.Configure<AuthenticationSecret>(builder.Configuration.GetSection("AuthenticationSecret"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// adding jwt support for swagger for testing purposes
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IQuestionsServices, QuestionsServices>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.Configure<DBConfiguration>(builder.Configuration.GetSection("Database"));

InitializationService.Init(builder.Configuration.GetValue<string>("Database:ConnectionString"));

var app = builder.Build();

app.UseCors(); // cors enabling

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();