using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using API.Models;

// Integrantes do Grupo:
// Enzo Xavier, Enzo Hashimoto, Lucas Vargas, Vinicius Menarim

var builder = WebApplication.CreateBuilder(args);

// Configuração Banco de Dados 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=trabalho.db"));

// Configuração CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configuração Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ativar CORS e Swagger
app.UseCors("PermitirTudo");
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "API Rodando! Acesse /swagger para ver a documentação.");

// ================= CRUD CATEGORIA =================

app.MapPost("/categorias/criar", async (Categoria novaCategoria, AppDbContext db) =>
{
    var validarResultado = new List<ValidationResult>();
    var validarContexto = new ValidationContext(novaCategoria);
    
    if (!Validator.TryValidateObject(novaCategoria, validarContexto, validarResultado, true))
    {
        var error = validarResultado.FirstOrDefault()?.ErrorMessage;
        return Results.BadRequest(error);
    }

    db.Categorias.Add(novaCategoria);
    await db.SaveChangesAsync();
    return Results.Created($"/categorias/{novaCategoria.Id}", novaCategoria);
});

app.MapGet("/categorias/listar", async (AppDbContext db) => 
    Results.Ok(await db.Categorias.ToListAsync()))
    .WithTags("Categorias");

app.MapPut("/categorias/atualizar/{id:int}", async (int id, [FromBody] Categoria atualizarCategoria, AppDbContext db) =>
{
    var validarResultado = new List<ValidationResult>();
    var validarContexto = new ValidationContext(atualizarCategoria);

    if (!Validator.TryValidateObject(atualizarCategoria, validarContexto, validarResultado, true))
    {
        var error = validarResultado.FirstOrDefault()?.ErrorMessage;
        return Results.BadRequest(error);
    }

    var categoria = await db.Categorias.FindAsync(id);
    if (categoria == null) return Results.NotFound();

    categoria.Nome = atualizarCategoria.Nome;
    categoria.Descricao = atualizarCategoria.Descricao;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/categorias/eliminar/{id:int}", async (int id, AppDbContext db) =>
{
    var categoria = await db.Categorias.FindAsync(id);
    if (categoria == null) return Results.NotFound();

    db.Categorias.Remove(categoria);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ================= CRUD CLIENTE =================

app.MapPost("/clientes/criar", async (Cliente novoCliente, AppDbContext db) =>
{
    var validarResultado = new List<ValidationResult>();
    var validarContexto = new ValidationContext(novoCliente);

    if (!Validator.TryValidateObject(novoCliente, validarContexto, validarResultado, true))
    {
        var error = validarResultado.FirstOrDefault()?.ErrorMessage;
        return Results.BadRequest(error);
    }

    db.Clientes.Add(novoCliente);
    await db.SaveChangesAsync();
    return Results.Created($"/clientes/{novoCliente.Id}", novoCliente);
});

app.MapGet("/clientes/listar", async (AppDbContext db) => 
    Results.Ok(await db.Clientes.ToListAsync()))
    .WithTags("Clientes");

app.MapPut("/clientes/atualizar/{id:int}", async (int id, [FromBody] Cliente atualizarCliente, AppDbContext db) =>
{
    var validarResultado = new List<ValidationResult>();
    var validarContexto = new ValidationContext(atualizarCliente);

    if (!Validator.TryValidateObject(atualizarCliente, validarContexto, validarResultado, true))
    {
        var error = validarResultado.FirstOrDefault()?.ErrorMessage;
        return Results.BadRequest(error);
    }

    var cliente = await db.Clientes.FindAsync(id);
    if (cliente == null) return Results.NotFound();

    cliente.Nome = atualizarCliente.Nome;
    cliente.Email = atualizarCliente.Email;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/clientes/eliminar/{id:int}", async (int id, AppDbContext db) =>
{
    var cliente = await db.Clientes.FindAsync(id);
    if (cliente == null) return Results.NotFound();

    db.Clientes.Remove(cliente);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ================= CRUD PRODUTO =================

app.MapPost("/produtos/criar", async (Produto novoProduto, AppDbContext db) =>
{
    var validarResultado = new List<ValidationResult>();
    var validarContexto = new ValidationContext(novoProduto);

    if (!Validator.TryValidateObject(novoProduto, validarContexto, validarResultado, true))
    {
        var error = validarResultado.FirstOrDefault()?.ErrorMessage;
        return Results.BadRequest(error);
    }

    db.Produtos.Add(novoProduto);
    await db.SaveChangesAsync();
    return Results.Created($"/produtos/{novoProduto.Id}", novoProduto);
});

app.MapGet("/produtos/listar", async (AppDbContext db) => 
    Results.Ok(await db.Produtos.ToListAsync()))
    .WithTags("Produtos");

app.MapPut("/produtos/atualizar/{id:int}", async (int id, [FromBody] Produto atualizarProduto, AppDbContext db) =>
{
    var validarResultado = new List<ValidationResult>();
    var validarContexto = new ValidationContext(atualizarProduto);

    if (!Validator.TryValidateObject(atualizarProduto, validarContexto, validarResultado, true))
    {
        var error = validarResultado.FirstOrDefault()?.ErrorMessage;
        return Results.BadRequest(error);
    }

    var produto = await db.Produtos.FindAsync(id);
    if (produto == null) return Results.NotFound();

    produto.Nome = atualizarProduto.Nome;
    produto.Preco = atualizarProduto.Preco;
    produto.Estoque = atualizarProduto.Estoque;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/produtos/eliminar/{id:int}", async (int id, AppDbContext db) =>
{
    var produto = await db.Produtos.FindAsync(id);
    if (produto == null) return Results.NotFound();

    db.Produtos.Remove(produto);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();