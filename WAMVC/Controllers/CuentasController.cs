using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WAMVC.Data;
using WAMVC.Models;
using System.Security.Claims;
using System.Security.Cryptography;

namespace WAMVC.Controllers
{
    public class CuentasController : Controller
    {
        private readonly ArtesaniasDBContext _context;
        private readonly ILogger<CuentasController> _logger;

        public CuentasController(ArtesaniasDBContext context, ILogger<CuentasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Cuentas/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Cuentas/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string nombre, string email, string password, string confirmPassword)
        {
            try
            {
                _logger.LogInformation("Iniciando registro para: {Email}", email);

                // Validar que las contraseñas coincidan
                if (password != confirmPassword)
                {
                    ModelState.AddModelError("", "Las contraseñas no coinciden");
                    return View();
                }

                // Validar que no exista el email
                var userExists = await _context.Usuarios
                    .AnyAsync(u => u.Email.ToLower() == email.ToLower());

                if (userExists)
                {
                    ModelState.AddModelError("Email", "Este email ya está registrado");
                    return View();
                }

                // Crear nuevo usuario
                var usuario = new Usuario
                {
                    Nombre = nombre,
                    Email = email,
                    PasswordHash = HashPassword(password),
                    Role = "User"
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Usuario registrado exitosamente: {Email}", email);

                // Iniciar sesión automáticamente después del registro
                await SignInUser(usuario);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario: {Email}", email);
                ModelState.AddModelError("", "Ocurrió un error al registrar el usuario: " + ex.Message);
                return View();
            }
        }

        // GET: /Cuentas/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Cuentas/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                _logger.LogInformation("Intento de login para: {Email}", email);

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("", "Email y contraseña son requeridos");
                    return View();
                }

                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

                if (usuario == null)
                {
                    ModelState.AddModelError("", "Email o contraseña incorrectos");
                    return View();
                }

                // Verificar contraseña
                if (!VerifyPassword(password, usuario.PasswordHash))
                {
                    ModelState.AddModelError("", "Email o contraseña incorrectos");
                    return View();
                }

                // Iniciar sesión
                await SignInUser(usuario);

                _logger.LogInformation("Login exitoso para: {Email}", email);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en login");
                ModelState.AddModelError("", "Ocurrió un error al iniciar sesión: " + ex.Message);
                return View();
            }
        }

        // POST: /Cuentas/Logout
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // GET: /Cuentas/AccessDenied
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task SignInUser(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(7)
                });
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string hash)
        {
            var computedHash = HashPassword(password);
            return computedHash == hash;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                var userCount = await _context.Usuarios.CountAsync();
                var users = await _context.Usuarios.ToListAsync();

                return Json(new
                {
                    canConnect,
                    userCount,
                    users = users.Select(u => new { u.Id, u.Nombre, u.Email, u.Role })
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
}
