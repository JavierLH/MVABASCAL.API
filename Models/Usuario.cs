using System;
using System.Collections.Generic;

namespace SistemaAduanero.API.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public int ClienteId { get; set; }

    public int RolId { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? UltimoAcceso { get; set; }

    public bool? Activo { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ICollection<ManifestacionesValor> ManifestacionesValors { get; set; } = new List<ManifestacionesValor>();

    public virtual Role Rol { get; set; } = null!;
}
