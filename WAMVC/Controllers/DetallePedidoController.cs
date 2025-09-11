using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WAMVC.Data;
using WAMVC.Models;

namespace WAMVC.Controllers
{
    public class DetallePedidoController : Controller
    {
        private readonly ArtesaniasDBContext _context;

        public DetallePedidoController(ArtesaniasDBContext context)
        {
            _context = context;
        }

        // GET: DetallePedido
        public async Task<IActionResult> Index()
        {
            var detalles = _context.DetallePedidos
                .Include(d => d.Pedido)
                    .ThenInclude(p => p.Cliente)
                .Include(d => d.Producto);
            return View(await detalles.ToListAsync());
        }

        // GET: DetallePedido/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallePedido = await _context.DetallePedidos
                .Include(d => d.Pedido)
                    .ThenInclude(p => p.Cliente)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detallePedido == null)
            {
                return NotFound();
            }

            return View(detallePedido);
        }

        // GET: DetallePedido/Create
        public IActionResult Create()
        {
            ViewData["IdPedido"] = new SelectList(_context.Pedidos.Include(p => p.Cliente), "Id", "Cliente.Nombre");
            ViewData["IdProducto"] = new SelectList(_context.Productos, "Id", "Nombre");
            return View();
        }

        // POST: DetallePedido/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdPedido,IdProducto,Cantidad,PrecioUnitario")] DetallePedidoModel detallePedido)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(detallePedido);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Detalle de pedido creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar el detalle: " + ex.Message);
            }
            ViewData["IdPedido"] = new SelectList(_context.Pedidos.Include(p => p.Cliente), "Id", "Cliente.Nombre", detallePedido.IdPedido);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "Id", "Nombre", detallePedido.IdProducto);
            return View(detallePedido);
        }

        // GET: DetallePedido/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallePedido = await _context.DetallePedidos.FindAsync(id);
            if (detallePedido == null)
            {
                return NotFound();
            }
            ViewData["IdPedido"] = new SelectList(_context.Pedidos.Include(p => p.Cliente), "Id", "Cliente.Nombre", detallePedido.IdPedido);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "Id", "Nombre", detallePedido.IdProducto);
            return View(detallePedido);
        }

        // POST: DetallePedido/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdPedido,IdProducto,Cantidad,PrecioUnitario")] DetallePedidoModel detallePedido)
        {
            if (id != detallePedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detallePedido);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Detalle de pedido actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetallePedidoExists(detallePedido.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPedido"] = new SelectList(_context.Pedidos.Include(p => p.Cliente), "Id", "Cliente.Nombre", detallePedido.IdPedido);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "Id", "Nombre", detallePedido.IdProducto);
            return View(detallePedido);
        }

        // GET: DetallePedido/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallePedido = await _context.DetallePedidos
                .Include(d => d.Pedido)
                    .ThenInclude(p => p.Cliente)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detallePedido == null)
            {
                return NotFound();
            }

            return View(detallePedido);
        }

        // POST: DetallePedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detallePedido = await _context.DetallePedidos.FindAsync(id);
            if (detallePedido != null)
            {
                _context.DetallePedidos.Remove(detallePedido);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Detalle de pedido eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        private bool DetallePedidoExists(int id)
        {
            return _context.DetallePedidos.Any(e => e.Id == id);
        }
    }
}