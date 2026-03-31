using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using API.Models;

//Integrantes do Grupo:
//Enzo Xavier
//Enzo Hashimoto
//Lucas Vargas
//Vinicius Menarim

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Dados locais mock para categorias
var categorias = new List<Categoria>
{
    new Categoria { Id = 1, Nome = "Sneakers Exclusivos", Descricao = "Tênis raros" },
    new Categoria { Id = 2, Nome = "Vestuário", Descricao = "Roupas de marca" }
};

// Dados locais mock para clientes
var clientes = new List<Cliente>
{
    new Cliente { Id = 1, Nome = "João Silva", Email = "joao.silva@example.com" },
    new Cliente { Id = 2, Nome = "Maria Oliveira", Email = "maria.oliveira@example.com" }
};

// Dados locais mock para produtos
var produtos = new List<Produto>
{
    new Produto { Id = 1, Nome = "Nike Air Force 1", Preco = 750.99, Estoque = 10 },
    new Produto { Id = 2, Nome = "Calça Baggy Streetwear", Preco = 200.00, Estoque = 25},
    new Produto { Id = 3, Nome = "Boné Vans", Preco = 159.99, Estoque = 5}
};


app.MapGet("/", () => "Hello World!");

//CRUD Categoria

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

//CRUD Cliente

#region Cliente - Criar
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

//CRUD Produto

#region Produto - Criar
app.MapPost("/produtos/criar", (Produto novoProduto) =>
{
    var validarResultado = new List<ValidationResult>();
    var validarContexto = new ValidationContext(novoProduto);

    if (!Validator.TryValidateObject(novoProduto, validarContexto, validarResultado, true))
    {
        var error = validarResultado.FirstOrDefault(r => r.MemberNames.Contains("Preco"))?.ErrorMessage
                    ?? validarResultado.FirstOrDefault()?.ErrorMessage;

        return Results.BadRequest(error);
    }

    novoProduto.Id = produtos.Count > 0 ? produtos.Max(p => p.Id) + 1 : 1;
    produtos.Add(novoProduto);
    return Results.Created($"/produtos/{novoProduto.Id}", novoProduto);
});
#endregion

#region Produto - Atualizar
app.MapPut("/produtos/atualizar/{id:int}", (int id, [FromBody] Produto atualizarProduto) =>
{
    var validarResultado = new List<ValidationResult>();
    var validarContexto = new ValidationContext(atualizarProduto);

    if (!Validator.TryValidateObject(atualizarProduto, validarContexto, validarResultado, true))
    {
        var error = validarResultado.FirstOrDefault(r => r.MemberNames.Contains("Preco"))?.ErrorMessage
                    ?? validarResultado.FirstOrDefault()?.ErrorMessage;

        return Results.BadRequest(error);
    }

    var produto = produtos.FirstOrDefault(p => p.Id == id);
    if (produto == null)
    {
        return Results.NotFound();
    }

    produto.Nome = atualizarProduto.Nome;
    produto.Preco = atualizarProduto.Preco;
    produto.Estoque = atualizarProduto.Estoque;

    return Results.NoContent();
});
#endregion

#region Produto - Listar
app.MapGet("/produtos/listar", () => Results.Ok(produtos))
    .WithName("GetProdutos")
    .WithTags("Produtos");
#endregion

#region Produto - Eliminar
app.MapDelete("/produtos/eliminar/{id:int}", (int id) =>
{
    var produto = produtos.FirstOrDefault(p => p.Id == id);
    if (produto == null)
    {
        return Results.NotFound();
    }

    produtos.Remove(produto);
    return Results.NoContent();
});
#endregion


app.Run();
