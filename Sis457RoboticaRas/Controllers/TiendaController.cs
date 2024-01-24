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
    public class TiendaController : Controller

    {
        private readonly RoboticaRasContext _context;

        public TiendaController(RoboticaRasContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var listaA = _context.Venta.ToList();
            var listaB = _context.VentaDetalles.ToList();
            return View();
        }
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Tienda tienda)
        {
            if (ModelState.IsValid)
            {
                var Venta = new Ventum
                {

                    IdUsuario = tienda.IdUsuario,
                    IdCliente = tienda.IdCliente,
                    TotalVenta = tienda.TotalVenta,
                    UsuarioRegistro = User.Identity?.Name,
                    FechaVenta = DateTime.Now,
                    Estado = 1
                    
                };
                _context.Venta.Add(Venta);
                _context.SaveChanges();

                var VentaDetalle = new VentaDetalle
                {
                    IdVenta = Venta.Id,
                    IdProducto = tienda.IdProducto,
                    Cantidad = tienda.Cantidad,
                    PrecioUnitario = tienda.PrecioUnitario,
                    Total = tienda.Total,
                    UsuarioRegistro = User.Identity?.Name,
                    FechaRegistro = DateTime.Now,
                    Estado = 1
                };
                _context.VentaDetalles.Add(VentaDetalle);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(tienda);
        }


    }
}
