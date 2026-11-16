using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Services;

namespace ClinicaVeterinaria.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext context, CpfProtectorService cpfService)
    {
        context.Database.EnsureCreated();

        if (context.Usuarios.Any()) return; // Já tem dados, não duplica

        // 1. Três Usuários (Senha padrão para todos: 123456)
        var hash = BCrypt.Net.BCrypt.HashPassword("123456");
        context.Usuarios.AddRange(
            new Usuario { Nome = "Admin Clínica", Email = "admin@clinica.com", SenhaHash = hash },
            new Usuario { Nome = "Dra. Ana", Email = "ana@clinica.com", SenhaHash = hash },
            new Usuario { Nome = "Recepção João", Email = "joao@clinica.com", SenhaHash = hash }
        );
        context.SaveChanges();

        // 2. Três Tutores (Com CPF criptografado!)
        var t1 = new Tutor { Nome = "Carlos Silva", CpfCriptografado = cpfService.Criptografar("11122233344"), Telefone = "54999991111", Email = "carlos@email.com" };
        var t2 = new Tutor { Nome = "Mariana Souza", CpfCriptografado = cpfService.Criptografar("55566677788"), Telefone = "54999992222", Email = "mariana@email.com" };
        var t3 = new Tutor { Nome = "Roberto Alves", CpfCriptografado = cpfService.Criptografar("99988877766"), Telefone = "54999993333", Email = "roberto@email.com" };
        context.Tutores.AddRange(t1, t2, t3);
        context.SaveChanges();

        // 3. Três Pets (Associados aos tutores)
        var p1 = new Pet { Nome = "Rex", Especie = "Cachorro", Raca = "Labrador", TutorId = t1.Id };
        var p2 = new Pet { Nome = "Mia", Especie = "Gato", Raca = "Siamês", TutorId = t2.Id };
        var p3 = new Pet { Nome = "Thor", Especie = "Cachorro", Raca = "Bulldog", TutorId = t3.Id };
        context.Pets.AddRange(p1, p2, p3);
        context.SaveChanges();

        // 4. Três Agendamentos
        context.Agendamentos.AddRange(
            new Agendamento { DataHora = DateTime.Now.AddDays(1), Motivo = "Vacina V10", TutorId = t1.Id, PetId = p1.Id },
            new Agendamento { DataHora = DateTime.Now.AddDays(2), Motivo = "Consulta de Rotina", TutorId = t2.Id, PetId = p2.Id },
            new Agendamento { DataHora = DateTime.Now.AddDays(3), Motivo = "Exame de Sangue", TutorId = t3.Id, PetId = p3.Id }
        );
        context.SaveChanges();
    }
}