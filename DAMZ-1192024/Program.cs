using DAMZ_1192024.Auth;
using DAMZ_1192024.EndPoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddAuthorization(options => {
    options.AddPolicy("LoggedInPolicy", policy => {
        policy.RequireAuthenticatedUser();
    });
});

var key = "Key.JWTAPIMinimal2024.API.Securekey2024";

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            var result = System.Text.Json.JsonSerializer.Serialize(new { message = "Debes iniciar sesión" });
            return context.Response.WriteAsync(result);
        }
    };
});

builder.Services.AddSingleton<IJwtAuthenticationService>(new JwtAuthenticationService(key));

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapPost("/protected-endpoint", [Authorize(Policy = "LoggedInPolicy")] (string data) => {
    return Results.Ok("Acceso concedido");
});

app.MapGet("/another-protected-endpoint", [Authorize(Policy = "LoggedInPolicy")] () => {
    return Results.Ok("Acceso concedido");
});


app.AddAccountEndPoint();
app.AddBodegaEndPoint();
app.AddCategoriaProductoEndPoint();

app.Run();