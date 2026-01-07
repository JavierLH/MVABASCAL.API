using System;
using System.Collections.Generic;

namespace SistemaAduanero.API.Models;

public partial class ClienteCredencialesVucem
{
    public int CredencialId { get; set; }

    public int ClienteId { get; set; }

    public string RfcVucem { get; set; } = null!;

    public string PasswordVucem { get; set; } = null!;

    public byte[]? ArchivoCer { get; set; }

    public byte[]? ArchivoKey { get; set; }

    public string? PasswordKey { get; set; }

    public DateTime? VigenciaInicio { get; set; }

    public DateTime? VigenciaFin { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;
}
