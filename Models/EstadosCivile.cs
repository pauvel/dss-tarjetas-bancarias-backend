using System;
using System.Collections.Generic;

#nullable disable

namespace dss_credito_bancario_backend.Models
{
    public partial class EstadosCivile
    {
        public EstadosCivile()
        {
            Clientes = new HashSet<Cliente>();
        }

        public int Id { get; set; }
        public string Tipo { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}
