using Microsoft.EntityFrameworkCore;
using DaccApi.Infrastructure.DataBaseContext;
using DaccApi.Services.User;
using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Infrastructure.Cryptography;
using System.Data;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IRepositoryDapper, RepositoryDapper>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IArgon2Utility, Argon2Utility>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
