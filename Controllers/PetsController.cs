using ClinicaVeterinaria.Data;
using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Controllers;

[Authorize]
public class PetsController : Controller
{
    private readonly AppDbContext _context;
    public PetsController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var pets = await _context.Pets.Include(p => p.Tutor).ToListAsync();
        return View(pets);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Tutores = new SelectList(await _context.Tutores.ToListAsync(), "Id", "Nome");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Pet pet)
    {
        ModelState.Remove("Tutor"); // Evita erro de validação no objeto de navegação
        if (!ModelState.IsValid)
        {
            ViewBag.Tutores = new SelectList(await _context.Tutores.ToListAsync(), "Id", "Nome");
            return View(pet);
        }

        _context.Pets.Add(pet);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}