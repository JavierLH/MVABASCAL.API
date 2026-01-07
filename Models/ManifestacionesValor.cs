using System;
using System.Collections.Generic;

namespace SistemaAduanero.API.Models;

public partial class ManifestacionesValor
{
    public int ManifestacionId { get; set; }

    public int ClienteId { get; set; }

    public int UsuarioCreadorId { get; set; }

    public string? TipoOperacion { get; set; }

    public bool ExisteVinculacion { get; set; }

    public string? NumeroPedimento { get; set; }

    public string? Patente { get; set; }

    public string? Aduana { get; set; }

    public string? Cove { get; set; } = null!;

    public string? Incoterm { get; set; }

    public string? MetodoValoracion { get; set; } = null!;

    public decimal? TotalPrecioPagado { get; set; }

    public decimal? TotalPrecioPorPagar { get; set; }

    public decimal? TotalIncrementables { get; set; }

    public decimal? TotalDecrementables { get; set; }

    public decimal? TotalValorAduana { get; set; }

    public string? CadenaOriginal { get; set; }

    public string? SelloDigital { get; set; }

    public DateTime? FechaFirma { get; set; }

    public string? NumeroOperacionVucem { get; set; }

    public string? EstadoEnvio { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Activo { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ICollection<ManifestacionConceptosValor> ManifestacionConceptosValors { get; set; } = new List<ManifestacionConceptosValor>();

    public virtual ICollection<ManifestacionConsultaRfc> ManifestacionConsultaRfcs { get; set; } = new List<ManifestacionConsultaRfc>();

    public virtual ICollection<ManifestacionCove> ManifestacionCoves { get; set; } = new List<ManifestacionCove>();

    public virtual ICollection<ManifestacionEdocument> ManifestacionEdocuments { get; set; } = new List<ManifestacionEdocument>();

    public virtual ICollection<ManifestacionPago> ManifestacionPagos { get; set; } = new List<ManifestacionPago>();

    public virtual Usuario UsuarioCreador { get; set; } = null!;
}
