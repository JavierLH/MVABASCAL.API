using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaAduanero.API.Models.Catalogos
{
    
    
        // Clase padre para no repetir código
        public abstract class CatalogoBase
        {
            [Key]
            [Column(TypeName = "varchar(20)")]
            public string Clave { get; set; } = null!;

            [Required]
            [Column(TypeName = "varchar(MAX)")] // Usamos MAX para cubrir descripciones largas
            public string Descripcion { get; set; } = null!;

            public bool Activo { get; set; } = true;
        }

        // Mapeo exacto de tus tablas SQL
        [Table("CatIncoterms")]
        public class CatIncoterm : CatalogoBase { }

        [Table("CatMetodosValoracion")]
        public class CatMetodoValoracion : CatalogoBase { }

        [Table("CatFormasPago")]
        public class CatFormaPago : CatalogoBase { }

        [Table("CatIncrementables")]
        public class CatIncrementable : CatalogoBase { }

        [Table("CatDecrementables")]
        public class CatDecrementable : CatalogoBase { }

        [Table("CatTipoFigura")]
        public class CatTipoFigura : CatalogoBase { }
    
}
