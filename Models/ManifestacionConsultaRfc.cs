using System;
using System.Collections.Generic;

namespace SistemaAduanero.API.Models;

public partial class ManifestacionConsultaRfc
{
    public int Id { get; set; }

    public int ManifestacionId { get; set; }

    public string RfcConsulta { get; set; } = null!;

    public string? TipoFigura { get; set; }

    public virtual ManifestacionesValor Manifestacion { get; set; } = null!;
}
