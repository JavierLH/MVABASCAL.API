using System;
using System.Collections.Generic;

namespace SistemaAduanero.API.Models;

public partial class ManifestacionEdocument
{
    public int AnexoId { get; set; }

    public int ManifestacionId { get; set; }

    public string NumeroEdocument { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ManifestacionesValor Manifestacion { get; set; } = null!;
}
