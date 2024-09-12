using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace DAMZ_1192024.EndPoints
{
    public static class ProtectedEndPoint
    {
        private static List<object> data = new List<object>();

        public static void AddProtectedEndPoint(this WebApplication app)
        {
            app.MapGet("/protected", () =>
            {
                return data;
            })
            .RequireAuthorization();

            app.MapPost("/protected", (string name, string lastName) =>
            {
                data.Add(new { Name = name, LastName = lastName });
                return Results.Ok();
            })
            .RequireAuthorization(); // Requiere autorización para acceder al endpoint
        }
    }
}