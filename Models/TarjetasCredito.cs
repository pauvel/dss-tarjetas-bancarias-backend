using System;
using System.Collections.Generic;

#nullable disable

namespace dss_credito_bancario_backend.Models
{
    public partial class TarjetasCredito
    {
        public TarjetasCredito()
        {
            Solicitudes = new HashSet<Solicitude>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public decimal? LimiteCredito { get; set; }
        public decimal? TasaInteresAnual { get; set; }
        public decimal? MinIngresoAcumulable { get; set; }

        public virtual ICollection<Solicitude> Solicitudes { get; set; }
    }
}
