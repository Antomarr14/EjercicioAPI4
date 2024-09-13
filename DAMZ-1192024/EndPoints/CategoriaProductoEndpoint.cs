using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace DAMZ_1192024.EndPoints
{
    public static class CategoriaProductoEndPoint
    {
        static List<CategoriaProducto> data = new List<CategoriaProducto>();
        static int nextId = 1;

        public static void AddCategoriaProductoEndPoint(this WebApplication app)
        {
            app.MapGet("/categorias", () =>
            {
                return data;
            }).RequireAuthorization();

            app.MapPost("/categorias", (string nombre, string descripcion) =>
            {
                var nuevaCategoria = new CategoriaProducto
                {
                    Id = nextId++,
                    Nombre = nombre,
                    Descripcion = descripcion
                };
                data.Add(nuevaCategoria);
                return Results.Ok(data);
            }).RequireAuthorization();

            app.MapDelete("/categorias/{id}", (int id) =>
            {
                var categoria = data.Find(c => c.Id == id);
                if (categoria != null)
                {
                    data.Remove(categoria);
                    return Results.Ok(new { message = "Se eliminó correctamente" });
                }
                else
                {
                    return Results.NotFound(new { message = "Categoría no encontrada" });
                }
            }).RequireAuthorization();
        }
    }

    public class CategoriaProducto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}