using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Categoria
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 80 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Descrição deve ter no máximo 200 caracteres.")]
    public string? Descricao { get; set; }
}