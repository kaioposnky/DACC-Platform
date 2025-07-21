using Microsoft.EntityFrameworkCore;
using DaccApi.Infrastructure.DataBaseContext;
using DaccApi.Services.User;
using DaccApi.Services.Auth;
using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Infrastructure.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Data;
using Npgsql;
using Microsoft.OpenApi.Models;
using DaccApi;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Infrastructure.Repositories.Avaliacao;
using DaccApi.Infrastructure.Repositories.Carrinhos;
using DaccApi.Services.Products;
using DaccApi.Infrastructure.Repositories.Products;
using DaccApi.Services.Diretorias;
using DaccApi.Infrastructure.Repositories.Diretorias;
using DaccApi.Infrastructure.Repositories.Noticias;
using DaccApi.Infrastructure.Repositories.Permission;
using DaccApi.Infrastructure.Repositories.Posts;
using DaccApi.Infrastructure.Repositories.Projetos;
using DaccApi.Services.Avaliacao;
using DaccApi.Services.Noticias;
using DaccApi.Services.Permission;
using DaccApi.Services.Posts;
using DaccApi.Services.Projetos;
using DaccApi.Services.Token;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DaccApi",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IRepositoryDapper, RepositoryDapper>();
builder.Services.AddScoped<IArgon2Utility, Argon2Utility>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IProdutosService, ProdutosService>();
builder.Services.AddScoped<IProdutosRepository, ProdutosRepository>();
builder.Services.AddScoped<IDiretoriasService, DiretoriasService>();
builder.Services.AddScoped<IDiretoriasRepository, DiretoriasRepository>();
builder.Services.AddScoped<IProjetosService, ProjetosService>();
builder.Services.AddScoped<IProjetosRepository, ProjetosRepository>();
builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>();
builder.Services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();
builder.Services.AddScoped<INoticiasRepository, NoticiasRepository>();
builder.Services.AddScoped<INoticiasServices, NoticiasServices>();
builder.Services.AddScoped<ICarrinhoRepository, CarrinhoRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IPostsServices, PostsServices>();
builder.Services.AddScoped<IPostsRepository, PostsRepository>();

var app = builder.Build();

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
