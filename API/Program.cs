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

var clientes = new List<Cliente>
{
    new Cliente { Id = 1, Nome = "João Silva", Email = "joao.silva@example.com" },
    new Cliente { Id = 2, Nome = "Maria Oliveira", Email = "maria.oliveira@example.com" }
};


app.MapGet("/", () => "Hello World!");

#region Categoria - Criar
app.MapPost("/categorias/criar", (Categoria novaCategoria) =>
{
    var validarResultado = new List<ValidationResult>();
    var validarContexto = new ValidationContext(novaCategoria);
    
    if (!Validator.TryValidateObject(novaCategoria, validarContexto, validarResultado, true))
    {
        var error = validarResultado.FirstOrDefault(r => r.MemberNames.Contains("Descricao"))?.ErrorMessage 
                    ?? validarResultado.FirstOrDefault()?.ErrorMessage;
        
        return Results.BadRequest(error);
    }

    novaCategoria.Id = categorias.Count > 0 ? categorias.Max(c => c.Id) + 1 : 1;
    categorias.Add(novaCategoria);
    return Results.Created($"/categorias/{novaCategoria.Id}", novaCategoria);
});
#endregion

#region Categoria - Listar
app.MapGet("/categorias/listar", () => Results.Ok(categorias))
    .WithName("GetCategorias")
    .WithTags("Categorias");
#endregion

#region Categoria - Atualizar
app.MapPut("/categorias/atualizar/{id:int}", (int id, [FromBody] Categoria atualizarCategoria) =>
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

#region Categoria - Eliminar
app.MapDelete("/categorias/eliminar/{id:int}", (int id) =>
{
    var categoria = categorias.FirstOrDefault(c => c.Id == id);
    if (categoria == null)
    {
        return Results.NotFound();
    }

    categorias.Remove(categoria);
    return Results.NoContent();
});
#endregion

#region Cliente - criar
app.MapPost("/clientes/criar", (Cliente novoCliente) =>
{
    var validarResultado = new List<ValidationResult>();
    var validarContexto = new ValidationContext(novoCliente);
    
    if (!Validator.TryValidateObject(novoCliente, validarContexto, validarResultado, true))
    {
        var error = validarResultado.FirstOrDefault(r => r.MemberNames.Contains("Email"))?.ErrorMessage 
                    ?? validarResultado.FirstOrDefault()?.ErrorMessage;
        
        return Results.BadRequest(error);
    }

    novoCliente.Id = clientes.Count > 0 ? clientes.Max(c => c.Id) + 1 : 1;
    clientes.Add(novoCliente);
    return Results.Created($"/clientes/{novoCliente.Id}", novoCliente);
});
#endregion

#region Cliente - Atualizar
app.MapPut("/clientes/atualizar/{id:int}", (int id, [FromBody] Cliente atualizarCliente) =>
{
    var validarResultado = new List<ValidationResult>();
    var validarContexto = new ValidationContext(atualizarCliente);
    
    if (!Validator.TryValidateObject(atualizarCliente, validarContexto, validarResultado, true))
    {
        var error = validarResultado.FirstOrDefault(r => r.MemberNames.Contains("Email"))?.ErrorMessage 
                    ?? validarResultado.FirstOrDefault()?.ErrorMessage;
        
        return Results.BadRequest(error);
    }

    var cliente = clientes.FirstOrDefault(c => c.Id == id);
    if (cliente == null)
    {
        return Results.NotFound();
    }

    cliente.Nome = atualizarCliente.Nome;
    cliente.Email = atualizarCliente.Email;

    return Results.NoContent();
});
#endregion

#region Cliente - Listar
app.MapGet("/clientes/listar", () => Results.Ok(clientes))
    .WithName("GetClientes")
    .WithTags("Clientes");
#endregion

#region Cliente - Eliminar
app.MapDelete("/clientes/eliminar/{id:int}", (int id) =>
{
    var cliente = clientes.FirstOrDefault(c => c.Id == id);
    if (cliente == null)
    {
        return Results.NotFound();
    }

    clientes.Remove(cliente);
    return Results.NoContent();
});
#endregion


app.Run();
