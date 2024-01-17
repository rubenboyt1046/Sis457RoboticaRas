using System;
using System.Collections.Generic;

namespace Sis457RoboticaRas.Models;

public partial class Ventum
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdCliente { get; set; }

    public decimal TotalVenta { get; set; }

    public DateTime FechaVenta { get; set; }

    public string UsuarioRegistro { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public short Estado { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();
}
