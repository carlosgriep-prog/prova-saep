using ClinicaVeterinaria.Data;
using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Services;
using ClinicaVeterinaria.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Controllers;

[Authorize]
public class TutoresController : Controller
{
    private readonly AppDbContext _context;
    private readonly CpfProtectorService _cpfService;

    public TutoresController(AppDbContext context, CpfProtectorService cpfService)
    {
        _context = context;
        _cpfService = cpfService;
    }

    public async Task<IActionResult> Index(string? busca)
    {
        ViewBag.Busca = busca;
        var query = _context.Tutores.AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
            query = query.Where(t => t.Nome.Contains(busca) || t.Email.Contains(busca));

        var tutores = await query.ToListAsync();
        var lista = tutores.Select(t =>
        {
            var cpfLimpo = _cpfService.Descriptografar(t.CpfCriptografado);
            var mascarado = cpfLimpo.Length == 11 ? $"***.{cpfLimpo.Substring(3, 3)}.{cpfLimpo.Substring(6, 3)}-**" : cpfLimpo;
            return new TutorListaViewModel { Id = t.Id, Nome = t.Nome, CpfMascarado = mascarado, Telefone = t.Telefone, Email = t.Email };
        }).ToList();

        return View(lista);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(TutorFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var tutor = new Tutor
        {
            Nome = model.Nome,
            CpfCriptografado = _cpfService.Criptografar(model.Cpf),
            Telefone = model.Telefone,
            Email = model.Email
        };
        _context.Tutores.Add(tutor);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var tutor = await _context.Tutores.FindAsync(id);
        if (tutor == null) return NotFound();

        var model = new TutorFormViewModel
        {
            Id = tutor.Id,
            Nome = tutor.Nome,
            Cpf = _cpfService.Descriptografar(tutor.CpfCriptografado),
            Telefone = tutor.Telefone,
            Email = tutor.Email
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TutorFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var tutor = await _context.Tutores.FindAsync(model.Id);
        if (tutor == null) return NotFound();

        tutor.Nome = model.Nome;
        tutor.CpfCriptografado = _cpfService.Criptografar(model.Cpf);
        tutor.Telefone = model.Telefone;
        tutor.Email = model.Email;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}