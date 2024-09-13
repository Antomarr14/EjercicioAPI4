using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace DAMZ_1192024.EndPoints
{
    public static class BodegaEndPoint
    {
        static List<Bodega> data = new List<Bodega>();
        static int nextId = 1; 

        public static void AddBodegaEndPoint(this WebApplication app)
        {
            app.MapGet("/bodega", () =>
            {
                return data;
            }).RequireAuthorization();

            app.MapPost("/bodega", (string nombre, string descripcion) =>
            {
                var nuevaBodega = new Bodega
                {
                    Id = nextId++, 
                    Nombre = nombre,
                    Descripcion = descripcion
                };
                data.Add(nuevaBodega);
                return Results.Ok(data);
            }).RequireAuthorization();

            app.MapDelete("/bodega/{id}", (int id) =>
            {
                var bodega = data.Find(b => b.Id == id);
                if (bodega != null)
                {
                    data.Remove(bodega);
                    return Results.Ok(new { message = "Se eliminó correctamente" });
                }
                else
                {
                    return Results.NotFound(new { message = "Bodega no encontrada" });
                }
            }).RequireAuthorization();
        }
    }

    public class Bodega
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}