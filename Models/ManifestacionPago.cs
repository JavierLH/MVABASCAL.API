using System;
using System.Collections.Generic;

namespace SistemaAduanero.API.Models;

public partial class ManifestacionPago
{
    public int PagoId { get; set; }

    public int ManifestacionId { get; set; }

    public string TipoSeccion { get; set; } = null!;

    public DateOnly? FechaPago { get; set; }

    public string? FormaPago { get; set; }

    public string? EspecifiqueFormaPago { get; set; }

    public decimal ImporteTotal { get; set; }

    public string TipoMoneda { get; set; } = null!;

    public decimal TipoCambio { get; set; }

    public string? SituacionNoPago { get; set; }

    public string? MotivoCompenso { get; set; }

    public string? PrestacionMercancia { get; set; }

    public virtual ManifestacionesValor Manifestacion { get; set; } = null!;
}
