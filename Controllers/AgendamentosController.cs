using ClinicaVeterinaria.Data;
using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Controllers;

[Authorize]
public class AgendamentosController : Controller
{
    private readonly AppDbContext _context;
    public AgendamentosController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var lista = await _context.Agendamentos
            .Include(a => a.Tutor)
            .Include(a => a.Pet)
            .OrderBy(a => a.DataHora) // Exigência da folha: ordenado por dia
            .ToListAsync();
        return View(lista);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Tutores = new SelectList(await _context.Tutores.ToListAsync(), "Id", "Nome");
        ViewBag.Pets = new SelectList(await _context.Pets.ToListAsync(), "Id", "Nome");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Agendamento agendamento)
    {
        ModelState.Remove("Tutor");
        ModelState.Remove("Pet");
        if (!ModelState.IsValid)
        {
            ViewBag.Tutores = new SelectList(await _context.Tutores.ToListAsync(), "Id", "Nome");
            ViewBag.Pets = new SelectList(await _context.Pets.ToListAsync(), "Id", "Nome");
            return View(agendamento);
        }

        _context.Agendamentos.Add(agendamento);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var agendamento = await _context.Agendamentos.Include(a => a.Tutor).Include(a => a.Pet).FirstOrDefaultAsync(a => a.Id == id);
        if (agendamento == null) return NotFound();
        return View(agendamento);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, DateTime dataHora)
    {
        var agendamento = await _context.Agendamentos.FindAsync(id);
        if (agendamento == null) return NotFound();

        agendamento.DataHora = dataHora; // Exigência da folha: atualizar a data
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}