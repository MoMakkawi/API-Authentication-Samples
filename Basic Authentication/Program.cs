using Basic_Authentication;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BloggingContext>(options =>
    options.UseInMemoryDatabase("BloggingDB"));

var app = builder.Build();

// Seed data
app.Services
    .CreateScope()
    .ServiceProvider 
    .GetRequiredService<BloggingContext>()
    .SeedData();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.BlogEndpoints();

app.Run();