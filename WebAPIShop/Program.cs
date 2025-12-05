using Microsoft.EntityFrameworkCore;
using Repository;
using Servers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddDbContext<dbSHOPContext>(options => options.UseSqlServer
("Data Source=srv2\\pupils;Initial Catalog=215949413_SHOP;Integrated Security=True;Trust Server Certificate=True"));



// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
