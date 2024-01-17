using System;
using System.Collections.Generic;

namespace Sis457RoboticaRas.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string CedulaIdentidad { get; set; } = null!;

    public string Celular { get; set; } = null!;

    public string UsuarioRegistro { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public short Estado { get; set; }

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
