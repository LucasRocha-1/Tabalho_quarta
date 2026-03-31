using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Produto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLenght(120, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 120 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O preço é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que 0.")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "O estoque é obrigatório.")]
    [Range(0, int.MaxValue, ErrorMessage = "O estoque deve ser maior ou igual a 0.")]
    public int Estoque { get; set; }

}