using System;

namespace DAMZ_1192024.EndPoints;

public static class BodegaEndPoint
{
    static List<object> data = new List<object>();

    public static void AddBodegaEndPoint(this WebApplication app){
        app.MapGet("/bodega", () =>{
            return data;
        }).AllowAnonymous();

        app.MapPost("/bodega", (string nombre, string descripcion)=>{
            data.Add(new{nombre, descripcion});
            return Results.Ok(data);
        }).RequireAuthorization();

        app.MapDelete("/bodega", ()=>{
            data = new List<object>();
            return Results.Ok(data);
        }).RequireAuthorization();
    }
}
