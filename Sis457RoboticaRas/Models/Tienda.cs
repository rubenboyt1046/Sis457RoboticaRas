using System;
using System.Collections.Generic;

namespace Sis457RoboticaRas.Models;

public partial class Tienda
{
    public int IdUsuario { get; set; }

    public int IdCliente { get; set; }

    public decimal TotalVenta { get; set; }

    public DateTime FechaVenta { get; set; }
    public int IdVenta { get; set; }

    public int IdProducto { get; set; }
    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Total { get; set; }

    public string UsuarioRegistro { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public short Estado { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;


}
