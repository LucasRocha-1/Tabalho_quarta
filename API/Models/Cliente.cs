using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(80, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 e 80 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O Email deve ser válido.")]
    public string Email { get; set; } = string.Empty;
}