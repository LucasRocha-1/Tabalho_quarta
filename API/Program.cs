using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using API.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// dados locais mock para categorias
var categorias = new List<Categoria>
{
    new Categoria { Id = 1, Nome = "Sneakers Exclusivos", Descricao = "Tênis raros" },
    new Categoria { Id = 2, Nome = "Vestuário", Descricao = "Roupas de marca" }
};

app.MapGet("/", () => "Hello World!");

#region Categoria

app.MapPut("/categorias/{id:int}", (int id, [FromBody] Categoria atualizarCategoria) =>
{
    var validarResultado = new List<ValidationResult>();
    var validarContexto = new ValidationContext(atualizarCategoria);
    
    if (!Validator.TryValidateObject(atualizarCategoria, validarContexto, validarResultado, true))
    {
        var error = validarResultado.FirstOrDefault(r => r.MemberNames.Contains("Descricao"))?.ErrorMessage 
                    ?? validarResultado.FirstOrDefault()?.ErrorMessage;
        
        return Results.BadRequest(error);
    }

    var categoria = categorias.FirstOrDefault(c => c.Id == id);
    if (categoria == null)
    {
        return Results.NotFound();
    }

    categoria.Nome = atualizarCategoria.Nome;
    categoria.Descricao = atualizarCategoria.Descricao;

    return Results.NoContent();
});
#endregion

app.Run();
