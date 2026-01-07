using System;
using System.Collections.Generic;

namespace SistemaAduanero.API.Models;

public partial class ManifestacionCove
{
    public int CoveId { get; set; }

    public int ManifestacionId { get; set; }

    public string NumeroCove { get; set; } = null!;

    public string? Incoterm { get; set; }

    public bool? ExisteVinculacion { get; set; }

    public string? NumeroPedimento { get; set; }

    public int? Patente { get; set; }

    public int? Aduana { get; set; }

    public DateTime? FechaPago { get; set; }

    public decimal? TotalPago { get; set; }

    public string? TipoPago { get; set; }

    public string? MonedaPago { get; set; }

    public decimal? TipoCambioPago { get; set; }

    public string? MetodoValoracion { get; set; }

    public decimal? TotalPrecioPagado { get; set; }

    public decimal? TotalPrecioPorPagar { get; set; }

    public decimal? TotalIncrementables { get; set; }

    public decimal? TotalDecrementables { get; set; }

    public decimal? TotalValorAduana { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Activo { get; set; }

    public virtual ManifestacionesValor Manifestacion { get; set; } = null!;
    public virtual ICollection<ManifestacionConceptosValor> ManifestacionConceptosValor { get; set; } = new List<ManifestacionConceptosValor>();
}
