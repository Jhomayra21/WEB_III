using Microsoft.AspNetCore.Mvc;
using WAMVC.Data;
using WAMVC.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace WAMVC.Controllers
{
 public class CuentasController : Controller
 {
 private readonly ArtesaniasDBContext _context;
 private readonly ILogger<CuentasController> _logger;
 private readonly IWebHostEnvironment _env;

 public CuentasController(ArtesaniasDBContext context, ILogger<CuentasController> logger, IWebHostEnvironment env)
 {
 _context = context;
 _logger = logger;
 _env = env;
 }

 // GET: /Cuentas/Register
 [HttpGet]
 public IActionResult Register()
 {
 return View();
 }

 // POST: /Cuentas/Register
 [HttpPost]
 [ValidateAntiForgeryToken]
 public async Task<IActionResult> Register(Usuario model, string password)
 {
 _logger.LogInformation("Register POST called for Email={Email}", model?.Email);

 // validar contraseña enviada
 if (string.IsNullOrEmpty(password))
 {
 ModelState.AddModelError("", "La contraseña es requerida.");
 }

 if (ModelState.IsValid)
 {
 try
 {
 var exists = await _context.Usuarios.AnyAsync(u => u.Email == model.Email);
 if (exists)
 {
 ModelState.AddModelError("Email", "El email ya está registrado.");
 return View(model);
 }

 model.PasswordHash = HashPassword(password);
 model.Role = string.IsNullOrEmpty(model.Role) ? "User" : model.Role;
 _context.Usuarios.Add(model);
 await _context.SaveChangesAsync();

 _logger.LogInformation("Usuario creado Id={Id} Email={Email}", model.Id, model.Email);

 await SignInUser(model);
 return RedirectToAction("Index", "Home");
 }
 catch (DbUpdateException dbEx)
 {
 _logger.LogError(dbEx, "Error al guardar en la base de datos para Email={Email}", model?.Email);
 ModelState.AddModelError("", "Error al guardar en la base de datos: " + dbEx.GetBaseException().Message);
 return View(model);
 }
 catch (System.Exception ex)
 {
 _logger.LogError(ex, "Error inesperado en Register para Email={Email}", model?.Email);
 ModelState.AddModelError("", "Error inesperado: " + ex.GetBaseException().Message);
 return View(model);
 }
 }
 return View(model);
 }

 // GET: /Cuentas/Login
 [HttpGet]
 public IActionResult Login()
 {
 return View();
 }

 // POST: /Cuentas/Login
 [HttpPost]
 [ValidateAntiForgeryToken]
 public async Task<IActionResult> Login(string email, string password)
 {
 _logger.LogInformation("Login POST called for Email={Email}", email);

 if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
 {
 ModelState.AddModelError(string.Empty, "Email y contraseña son requeridos.");
 return View();
 }

 var user = await _context.Usuarios.SingleOrDefaultAsync(u => u.Email == email);
 if (user == null || !VerifyPassword(password, user.PasswordHash))
 {
 ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
 return View();
 }

 await SignInUser(user);
 return RedirectToAction("Index", "Home");
 }

 [Authorize]
 public async Task<IActionResult> Logout()
 {
 await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
 return RedirectToAction("Login", "Cuentas");
 }

 // Acción de depuración para verificar conexión y conteo de usuarios (solo en Development)
 [HttpGet]
 public async Task<IActionResult> Debug()
 {
 if (!_env.IsDevelopment())
 {
 return NotFound();
 }
 try
 {
 var canConnect = await _context.Database.CanConnectAsync();
 var count = await _context.Usuarios.CountAsync();
 return Content($"CanConnect={canConnect}; Usuarios={count}");
 }
 catch (System.Exception ex)
 {
 _logger.LogError(ex, "Error en Debug");
 return Content("Error: " + ex.GetBaseException().Message);
 }
 }

 private async Task SignInUser(Usuario user)
 {
 var claims = new List<Claim>
 {
 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
 new Claim(ClaimTypes.Name, user.Nombre ?? user.Email),
 new Claim(ClaimTypes.Email, user.Email),
 new Claim(ClaimTypes.Role, user.Role ?? "User")
 };

 var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
 var principal = new ClaimsPrincipal(identity);
 await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
 }

 // Simple password hashing using SHA256 (for demo only). For production, use ASP.NET Core Identity or a stronger KDF (e.g., PBKDF2/BCrypt/Argon2)
 private string HashPassword(string password)
 {
 using var sha = SHA256.Create();
 var bytes = System.Text.Encoding.UTF8.GetBytes(password);
 var hash = sha.ComputeHash(bytes);
 return Convert.ToBase64String(hash);
 }

 private bool VerifyPassword(string password, string storedHash)
 {
 var hash = HashPassword(password);
 return hash == storedHash;
 }
 }
}
