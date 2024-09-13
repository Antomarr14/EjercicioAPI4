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
                
                if (login == "admin" && password == "admin123")
                {
                    
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