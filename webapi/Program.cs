using System.Data;
using System.Net;
using System.Text;
using System.Threading.RateLimiting;
using DaccApi.Helpers;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Infrastructure.BackgroundServices;
using DaccApi.Infrastructure.Cryptography;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Infrastructure.Mail;
using DaccApi.Infrastructure.MercadoPago.Services;
using DaccApi.Infrastructure.MercadoPago.Services.DaccApi.Infrastructure.MercadoPago.Services;
using DaccApi.Infrastructure.Repositories.Anuncio;
using DaccApi.Infrastructure.Repositories.Avaliacao;
using DaccApi.Infrastructure.Repositories.Diretores;
using DaccApi.Infrastructure.Repositories.Eventos;
using DaccApi.Infrastructure.Repositories.Noticias;
using DaccApi.Infrastructure.Repositories.Orders;
using DaccApi.Infrastructure.Repositories.Orders.DaccApi.Infrastructure.Repositories.Orders;
using DaccApi.Infrastructure.Repositories.Permission;
using DaccApi.Infrastructure.Repositories.Posts;
using DaccApi.Infrastructure.Repositories.Products;
using DaccApi.Infrastructure.Repositories.Projetos;
using DaccApi.Infrastructure.Repositories.Reservas;
using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Infrastructure.Services.MercadoPago;
using DaccApi.Middlewares;
using DaccApi.Responses;
using DaccApi.Services.Anuncios;
using DaccApi.Services.Auth;
using DaccApi.Services.Avaliacao;
using DaccApi.Services.Diretores;
using DaccApi.Services.Eventos;
using DaccApi.Services.FileStorage;
using DaccApi.Services.Noticias;
using DaccApi.Services.Orders;
using DaccApi.Services.Permission;
using DaccApi.Services.Posts;
using DaccApi.Services.Products;
using DaccApi.Services.Projetos;
using DaccApi.Services.Token;
using DaccApi.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            ),
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DaccApi", Version = "v1" });

    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Insira o token JWT no formato: Bearer {seu_token}",
        }
    );

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                Array.Empty<string>()
            },
        }
    );

    // Adiciona o filtro para processar os atributos de resposta customizados
    c.OperationFilter<DaccApi.Helpers.Attributes.ApiResponseOperationFilter>();
});

builder.Services.AddHttpContextAccessor();
builder
    .Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var validationErrors = context
                .ModelState.SelectMany(m =>
                    m.Value.Errors.Select(e => new ResponseError.ValidationErrorDetail
                    {
                        Field = char.ToLowerInvariant(m.Key[0]) + m.Key[1..],
                        Message = e.ErrorMessage,
                    })
                )
                .ToList();

            var responseError = ResponseError.VALIDATION_ERROR.WithDetails(
                validationErrors.ToArray()
            );
            return new ObjectResult(responseError) { StatusCode = responseError.StatusCode };
        };
    });

builder.Services.AddRateLimiter(options =>
{
    var rateLimitInterval = builder.Configuration.GetValue<int>("RateLimit:IntervalMinutes", 2);
    var rateLimitMaxRequests = builder.Configuration.GetValue<int>("RateLimit:MaxRequests", 100);

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, IPAddress>(context =>
    {
        var clientIp = context.Connection.RemoteIpAddress ?? IPAddress.Loopback;
        return RateLimitPartition.GetFixedWindowLimiter(
            clientIp,
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = rateLimitMaxRequests,
                Window = TimeSpan.FromMinutes(rateLimitInterval),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0,
            }
        );
    });

    options.OnRejected = async (context, rateLimiterRule) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        var response = ResponseError.RATE_LIMIT_EXCEEDED;
        await ResponseHelper.WriteResponseErrorAsync(
            context.HttpContext,
            HttpStatusCode.TooManyRequests,
            response
        );
    };
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(
    builder.Configuration.GetConnectionString("DefaultConnection")
));
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
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddHostedService<ReservationCleanupService>();

var app = builder.Build();

app.UseMiddleware<LoggerMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

var contentRootPath = builder.Environment.ContentRootPath;
var webRootPath = Path.Combine(contentRootPath, "wwwroot");

if (!Directory.Exists(webRootPath))
{
    Directory.CreateDirectory(webRootPath);
    app.Environment.WebRootPath = webRootPath;
}

var uploadFilesSubfolder = builder.Configuration["UploadFilesSubfolder"]!;
var uploadsPath = Path.Combine(app.Environment.WebRootPath, uploadFilesSubfolder);

if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseFileServer(
    new FileServerOptions
    {
        FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uploadsPath),
        RequestPath = $"/{uploadFilesSubfolder}",
        EnableDirectoryBrowsing = false,
    }
);

app.MapControllers();

app.Run();
