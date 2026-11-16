using ClinicaVeterinaria.Data;
using ClinicaVeterinaria.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Banco de Dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Criptografia do CPF (Data Protection)
builder.Services.AddDataProtection();
builder.Services.AddScoped<CpfProtectorService>();

// Login com Cookie e Tempo de Expiração (Exigência da prova)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20); // Expira em 20 minutos
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Cria o banco e insere os 3 registros iniciais automaticamente
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var cpfService = scope.ServiceProvider.GetRequiredService<CpfProtectorService>();
    DbInitializer.Seed(context, cpfService);
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();