using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Controllers;

[Authorize]
public class HomeController : Controller
{
    public IActionResult Index() => View();
}