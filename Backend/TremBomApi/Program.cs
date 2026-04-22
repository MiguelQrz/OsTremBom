using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
/*
// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5500", "http://127.0.0.1:5500", "https://localhost:5001")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Banco de Dados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "chave_super_secreta_trem_bom_2026_minimo_32_caracteres!!";
var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "TremBom",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "TremBomClient",
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// Injeção de Dependências
builder.Services.AddScoped<IAuthService, AuthService>();
*/
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddScoped<IPublicacaoService, PublicacaoService>();
/* 
Swagger não é funcional para nós, necessitamos de uma API em produção, e o Swagger é mais recomendado para APIs em desenvolvimento ou para documentação interna. 
Para uma API em produção, é melhor focar em uma documentação clara e acessível, como um README detalhado ou uma página de documentação dedicada, que pode ser hospedada junto com a API ou em um serviço de documentação online. 
Isso garante que os usuários da API tenham acesso às informações necessárias sem expor detalhes técnicos ou ferramentas de desenvolvimento que podem não ser relevantes para eles.

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TREM BOM API",
        Version = "v1",
        Description = "API para OS TREM DE BH - Autenticação e Usuários"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TREM BOM API V1");
    });
}
*/
builder.Services.AddDirectoryBrowser();
var app = builder.Build();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowFrontend");

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();