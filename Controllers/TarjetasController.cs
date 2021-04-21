using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using dss_credito_bancario_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dss_credito_bancario_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarjetasController : ControllerBase
    {
        private readonly bancarioContext db;
        public TarjetasController(bancarioContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TarjetasCredito>>> GetAll(){
            var tarjetas = await this.db.TarjetasCreditos.ToListAsync();
            return Ok(tarjetas);
        }

        [HttpGet]
        [Route("{tarjetaid}")]
        public async Task<ActionResult<TarjetasCredito>> GetById(int tarjetaid){
            var result = await this.db.TarjetasCreditos.FirstOrDefaultAsync(tarjeta => tarjeta.Id == tarjetaid);
            return Ok(result);
        }
    }
}