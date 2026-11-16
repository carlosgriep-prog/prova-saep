using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.ViewModels;

public class TutorFormViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Nome obrigatório")]
    public string Nome { get; set; } = string.Empty;
    [Required(ErrorMessage = "CPF obrigatório"), StringLength(11, MinimumLength = 11, ErrorMessage = "Digite os 11 números do CPF")]
    public string Cpf { get; set; } = string.Empty;
    [Required(ErrorMessage = "Telefone obrigatório")]
    public string Telefone { get; set; } = string.Empty;
    [Required(ErrorMessage = "E-mail obrigatório"), EmailAddress]
    public string Email { get; set; } = string.Empty;
}