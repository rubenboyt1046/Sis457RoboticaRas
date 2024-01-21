using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sis457RoboticaRas.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Sis457RoboticaRas.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly RoboticaRasContext _context;

        public AccountController(RoboticaRasContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Intentos de inicio de sesión no válidos.");
                return View(model);
            }

            var usuario = _context.Usuarios.Include(e => e.IdEmpleadoNavigation)
                .Where(x => x.Estado == 1 && x.Usuario1 == model.usuario &&
                    x.Clave == Util.Encrypt(model.clave)).FirstOrDefault();
            if (usuario != null)
            {
                TempData["isLogged"] = true;
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Usuario1),
                    new Claim("FullName", $"{usuario.IdEmpleadoNavigation.Nombre} {usuario.IdEmpleadoNavigation.Apellidos}"),
                    new Claim(ClaimTypes.Email, "asrrobotics@gmail.com"),
                    new Claim(ClaimTypes.Role, usuario.IdEmpleadoNavigation.Cargo, ClaimValueTypes.String)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(15),
                    IsPersistent = model.recordarme
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                if (returnUrl == null) returnUrl = ViewData["ReturnUrl"]?.ToString();
                if (returnUrl != null) return Redirect(returnUrl);
                else return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                ModelState.AddModelError("", "Intentos de inicio de sesión no válidos.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            TempData["isLogged"] = false;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

    }
}
