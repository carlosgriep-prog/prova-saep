using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Digite o e-mail"), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Digite a senha")]
    public string Senha { get; set; } = string.Empty;
}