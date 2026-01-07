using System;
using System.Collections.Generic;

namespace SistemaAduanero.API.Models;

public partial class Permiso
{
    public int PermisoId { get; set; }

    public string CodigoPermiso { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Role> Rols { get; set; } = new List<Role>();
}
