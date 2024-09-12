using System;
using DAMZ_1192024.Auth;


namespace DAMZ_1192024.EndPoints
{
    public static class AccountEndPoint 
    {
        public static void AddAccountEndPoint(this WebApplication app)
        {
            app.MapPost("/account/login", (string login, string password, IJwtAuthenticationService authService) =>
            {
                // Verificar las credenciales del usuario
                if (login == "admin" && password == "admin123")
                {
                    // Generar el token usando el nombre de usuario correcto
                    var token = authService.Authenticate(login);
                    return Results.Ok(new { Token = token });
                }
                else
                {
                    return Results.Unauthorized();
                }
            });
        }
    }
}