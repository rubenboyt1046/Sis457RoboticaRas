using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sis457RoboticaRas.Models;

namespace Sis457RoboticaRas.Controllers
{
    [Authorize]
    public class VentaDetallesController : Controller
    {
        private readonly RoboticaRasContext _context;

        public VentaDetallesController(RoboticaRasContext context)
        {
            _context = context;
        }

        // GET: VentaDetalles
        public async Task<IActionResult> Index()
        {
            var roboticaRasContext = _context.VentaDetalles.Where(x => x.Estado != -1).Include(v => v.IdProductoNavigation).Include(v => v.IdVentaNavigation);
            return View(await roboticaRasContext.ToListAsync());
        }

        // GET: VentaDetalles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VentaDetalles == null)
            {
                return NotFound();
            }

            var ventaDetalle = await _context.VentaDetalles
                .Include(v => v.IdProductoNavigation)
                .Include(v => v.IdVentaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ventaDetalle == null)
            {
                return NotFound();
            }

            return View(ventaDetalle);
        }

        // GET: VentaDetalles/Create
        public IActionResult Create()
        {
            ViewData["IdVenta"] = new SelectList(_context.Venta, "Id", "Id");

            //ViewData["IdProducto"] = new SelectList(_context.Productos.Where(x => x.Estado != -1 && x.Estado != 0).Select(x => new
            //{
               // x.IdProducto,
               // Nombre = $"{x.IdProducto}"
            //}).ToList(), "IdProducto", "Nombre");                      

            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre");
            
            return View();
        }

        // POST: VentaDetalles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdVenta,IdProducto,Cantidad,PrecioUnitario,Total")] VentaDetalle ventaDetalle)
        {
            if (!int.IsEvenInteger(ventaDetalle.IdVenta) || !int.IsEvenInteger(ventaDetalle.IdProducto))
            {
                ventaDetalle.UsuarioRegistro = User.Identity?.Name;
                ventaDetalle.FechaRegistro = DateTime.Now;
                ventaDetalle.Estado = 1;
               
                _context.Add(ventaDetalle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", ventaDetalle.IdProducto);
            ViewData["IdVenta"] = new SelectList(_context.Venta, "Id", "Id", ventaDetalle.IdVenta);           

            return View(ventaDetalle);
        }

        // GET: VentaDetalles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VentaDetalles == null)
            {
                return NotFound();
            }

            var ventaDetalle = await _context.VentaDetalles.FindAsync(id);
            if (ventaDetalle == null)
            {
                return NotFound();
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", ventaDetalle.IdProducto);
            ViewData["IdVenta"] = new SelectList(_context.Venta, "Id", "Id", ventaDetalle.IdVenta);
            return View(ventaDetalle);
        }

        // POST: VentaDetalles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdVenta,IdProducto,Cantidad,PrecioUnitario,Total,UsuarioRegistro,FechaRegistro,Estado")] VentaDetalle ventaDetalle)
        {
            if (id != ventaDetalle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ventaDetalle.UsuarioRegistro = User.Identity?.Name;
                    _context.Update(ventaDetalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaDetalleExists(ventaDetalle.Id))
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
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", ventaDetalle.IdProducto);
            ViewData["IdVenta"] = new SelectList(_context.Venta, "Id", "Id", ventaDetalle.IdVenta);
            return View(ventaDetalle);
        }

        // GET: VentaDetalles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VentaDetalles == null)
            {
                return NotFound();
            }

            var ventaDetalle = await _context.VentaDetalles
                .Include(v => v.IdProductoNavigation)
                .Include(v => v.IdVentaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ventaDetalle == null)
            {
                return NotFound();
            }

            return View(ventaDetalle);
        }

        // POST: VentaDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VentaDetalles == null)
            {
                return Problem("Entity set 'RoboticaRasContext.VentaDetalles'  is null.");
            }
            var ventaDetalle = await _context.VentaDetalles.FindAsync(id);
            if (ventaDetalle != null)
            {
                ventaDetalle.Estado = -1;
                ventaDetalle.UsuarioRegistro = User.Identity?.Name ?? "";
                //_context.VentaDetalles.Remove(ventaDetalle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaDetalleExists(int id)
        {
          return (_context.VentaDetalles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
