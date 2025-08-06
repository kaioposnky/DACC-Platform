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
using DaccApi.Infrastructure.Authentication;
using DaccApi.Infrastructure.Repositories.Avaliacao;
using DaccApi.Services.Products;
using DaccApi.Infrastructure.Repositories.Products;
using DaccApi.Services.Diretores;
using DaccApi.Infrastructure.Repositories.Diretores;
using DaccApi.Infrastructure.Repositories.Noticias;
using DaccApi.Infrastructure.Repositories.Permission;
using DaccApi.Infrastructure.Repositories.Posts;
using DaccApi.Infrastructure.Repositories.Projetos;
using DaccApi.Infrastructure.Repositories.Eventos;
using DaccApi.Infrastructure.Repositories.Anuncio;
using DaccApi.Infrastructure.Repositories.Orders;
using DaccApi.Infrastructure.Services.MercadoPago;
using DaccApi.Middleware;
using DaccApi.Services.Avaliacao;
using DaccApi.Services.Eventos;
using DaccApi.Services.FileStorage;
using DaccApi.Services.Noticias;
using DaccApi.Services.Permission;
using DaccApi.Services.Posts;
using DaccApi.Services.Projetos;
using DaccApi.Services.Token;
using DaccApi.Services.Anuncio;
using DaccApi.Services.Orders;
using Helpers.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var validationErrors = context.ModelState
            .SelectMany(m => m.Value.Errors
                .Select(e => new ResponseError.ValidationErrorDetail 
                {
                    Field = char.ToLowerInvariant(m.Key[0]) + m.Key[1..],
                    Message = e.ErrorMessage
                }))
            .ToList();

        var responseError = ResponseError.VALIDATION_ERROR.WithDetails(validationErrors.ToArray());
        
        var response = new ApiResponse(false, responseError.ErrorInfo);
        return new ObjectResult(response) { StatusCode = responseError.StatusCode };
    };
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IRepositoryDapper, RepositoryDapper>();
builder.Services.AddScoped<IArgon2Utility, Argon2Utility>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAnuncioService, AnuncioService>();
builder.Services.AddScoped<IAnuncioRepository, AnuncioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IProdutosService, ProdutosService>();
builder.Services.AddScoped<IProdutosRepository, ProdutosRepository>();
builder.Services.AddScoped<IDiretoresService, DiretoresService>();
builder.Services.AddScoped<IDiretoresRepository, DiretoresRepository>();
builder.Services.AddScoped<IProjetosService, ProjetosService>();
builder.Services.AddScoped<IProjetosRepository, ProjetosRepository>();
builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>();
builder.Services.AddScoped<IEventosService, EventosService>();
builder.Services.AddScoped<IEventosRepository, EventosRepository>();
builder.Services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();
builder.Services.AddScoped<INoticiasRepository, NoticiasRepository>();
builder.Services.AddScoped<INoticiasServices, NoticiasServices>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IPostsServices, PostsServices>();
builder.Services.AddScoped<IPostsRepository, PostsRepository>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IMercadoPagoService, MercadoPagoService>();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();


app.UseStaticFiles();

var contentRootPath = builder.Environment.ContentRootPath;
var webRootPath = Path.Combine(contentRootPath, "wwwroot");

if (!Directory.Exists(webRootPath)){
    Directory.CreateDirectory(webRootPath);
}

var uploadFilesSubfolder = builder.Configuration["UploadFilesSubfolder"]!;
var uploadsPath = Path.Combine(app.Environment.WebRootPath, uploadFilesSubfolder);

if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseFileServer(new FileServerOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uploadsPath),
    RequestPath = $"/{uploadFilesSubfolder}",
    EnableDirectoryBrowsing = false
});

app.MapControllers();


app.Run();

