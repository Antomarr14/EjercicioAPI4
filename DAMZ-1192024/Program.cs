using DAMZ_1192024.Auth;
using DAMZ_1192024.EndPoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(b => {
    b.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT API", Version = "V1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Ingresar tu token de JWT Authentication",
        
        Reference = new OpenApiReference {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    b.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    b.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
});

// Configuración de la política de autorización
builder.Services.AddAuthorization(options => {
    options.AddPolicy("LoggedInPolicy", policy => {
        policy.RequireAuthenticatedUser();
    });
});

// Clave secreta para JWT
var key = "Key.JWTAPIMinimal2024.API";

// Configuración de autenticación
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.RequireHttpsMetadata = false; // Cambiado de RequireHttpsMetadat a RequireHttpsMetadata
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
    };
});

// Registro del servicio de autenticación JWT
builder.Services.AddSingleton<IJwtAuthenticationService>(new JwtAuthenticationService(key));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Agregar endpoints
app.AddAccountEndPoint();
app.AddProtectedEndPoint();
app.AddBodegaEndPoint();

app.Run();