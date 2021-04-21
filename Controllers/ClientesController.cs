using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using dss_credito_bancario_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dss_credito_bancario_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly bancarioContext Db;
        
        public ClientesController(bancarioContext db)
        {
            Db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAll(){
            var clientes = await this.Db.Clientes
                                        .Include(cl => cl.IdEstadoCivilNavigation)
                                        .Include(cl => cl.Solicitudes)
                                        .OrderByDescending(cl => cl.Id)
                                        .ToListAsync();
            return Ok(clientes);
        }
    }
}