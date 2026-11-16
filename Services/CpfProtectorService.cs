using Microsoft.AspNetCore.DataProtection;

namespace ClinicaVeterinaria.Services;

public class CpfProtectorService
{
    private readonly IDataProtector _protector;

    public CpfProtectorService(IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector("ClinicaVeterinaria.Tutor.Cpf");
    }

    public string Criptografar(string cpf) => _protector.Protect(cpf);

    public string Descriptografar(string cpfCriptografado)
    {
        try { return _protector.Unprotect(cpfCriptografado); }
        catch { return "CPF Inválido/Erro"; }
    }
}