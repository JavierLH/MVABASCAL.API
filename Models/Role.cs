using System;
using System.Collections.Generic;

namespace SistemaAduanero.API.Models;

public partial class Role
{
    public int RolId { get; set; }

    public string NombreRol { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
}
