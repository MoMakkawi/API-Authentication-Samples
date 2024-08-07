using System.Text;

using Bearer_Authentication.Authentication.Services;
using Bearer_Authentication.Endpoints;
using Bearer_Authentication.Options;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Persistence.Contracts;
using Persistence.Data;
using Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtOptions = builder.Configuration
    .GetRequiredSection("JWT")
    .Get<JwtOptions>()!;

builder.Services
    .AddAuthorization()
    .AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            SaveSigninToken = true,

            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))

        };
    });

// Scope Services
builder.Services.AddSingleton(jwtOptions);
builder.Services.AddScoped<JwtAuthenticationService>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();

// Database Service
builder.Services.AddDbContext<BloggingContext>(options =>
    options.UseInMemoryDatabase("BloggingDB"));

var app = builder.Build();


// Seed data
app.Services.CreateScope()
    .ServiceProvider
    .GetRequiredService<BloggingContext>()
    .SeedUsers()
    .SeedBlogs();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapUserEndpoints();
app.MapBlogEndpoints();

app.Run();