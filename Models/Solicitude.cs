using System;
using System.Collections.Generic;

#nullable disable

namespace dss_credito_bancario_backend.Models
{
    public partial class Solicitude
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public DateTime? Fecha { get; set; }
        public bool Aprobado { get; set; }
        public int? IdTarjetaCredito { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual TarjetasCredito IdTarjetaCreditoNavigation { get; set; }
    }
}
