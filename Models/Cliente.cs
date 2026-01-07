using System;
using System.Collections.Generic;

namespace SistemaAduanero.API.Models;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string Rfc { get; set; } = null!;

    public DateTime? FechaRegistro { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<ClienteCredencialesVucem> ClienteCredencialesVucems { get; set; } = new List<ClienteCredencialesVucem>();

    public virtual ICollection<ManifestacionesValor> ManifestacionesValors { get; set; } = new List<ManifestacionesValor>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
