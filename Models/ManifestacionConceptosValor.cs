using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations.Schema; // <--- 1. IMPORTANTE AGREGAR ESTO
using System.Text.Json.Serialization; // <--- Para el JsonIgnore
namespace SistemaAduanero.API.Models;

public partial class ManifestacionConceptosValor
{
    public int ConceptoId { get; set; }

    public int? ManifestacionId { get; set; }
    public int? CoveId { get; set; }

    public string TipoConcepto { get; set; } = null!;

    public string ClaveConcepto { get; set; } = null!;

    public DateTime? FechaErogacion { get; set; }

    public decimal Importe { get; set; }

    public bool? AcargoImportador { get; set; }

    public string? TipoMoneda { get; set; }

    public decimal? TipoCambio { get; set; }


    [JsonIgnore]
    public virtual ManifestacionesValor? Manifestacion { get; set; }

    [ForeignKey("CoveId")] 
        [JsonIgnore] // Evitamos ciclos infinitos al serializar
    public virtual ManifestacionCove? ManifestacionCove { get; set; }
}
