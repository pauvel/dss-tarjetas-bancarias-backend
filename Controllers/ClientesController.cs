using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using dss_credito_bancario_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using dss_credito_bancario_backend.Models;

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

        public double GetTasa(int meses){
            if(meses == 12) return 55;
            if(meses == 6) return 27.5;
            if(meses == 3) return 17.75;
            return 0;
        }

        private async Task<(Solicitude solicitud, decimal? ingresoAcumulable)> ObtenerSugerido(int porcentaje, Cliente cliente, Solicitude solicitud){
            var ingresoAcumulable = cliente.IngresosMensuales * porcentaje / 100;
            var planSugerido = await Db.TarjetasCreditos
                                .Where(pl => pl.MinIngresoAcumulable <= ingresoAcumulable)
                                .OrderByDescending(pl => pl.LimiteCredito)
                                .ToListAsync();
            solicitud.IdCliente = cliente.Id;
            solicitud.Aprobado = true;
            solicitud.IdTarjetaCredito = ingresoAcumulable < 4000 ? 1 : planSugerido[0].Id;
            return (solicitud, ingresoAcumulable);
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

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Cliente>> GetClientById(int id){
            var cliente = await this.Db.Clientes
                                        .Include(cl => cl.IdEstadoCivilNavigation)
                                        .Include(cl => cl.Solicitudes)
                                        .FirstOrDefaultAsync(cl => cl.Id == id);
            return Ok(cliente);
        }

        [HttpGet]
        [Route("curp/{curp}")]
        public async Task<ActionResult<Cliente>> GetClientByCurp(string curp){
            var cliente = await this.Db.Clientes
                                        .Include(cl => cl.IdEstadoCivilNavigation)
                                        .Include(cl => cl.Solicitudes)
                                        .FirstOrDefaultAsync(cl => cl.Curp == curp);
            return Ok(cliente);
        }

        [HttpPost]
        [Route("{clienteId}")]
        public async Task<ActionResult<Solicitude>> MakePetition([FromRoute]int clienteId){
            var solicitud  = new Solicitude();
            var cliente = await this.Db.Clientes
                                    .Include(cl => cl.IdEstadoCivilNavigation)
                                    .Include(cl => cl.Solicitudes)
                                    .FirstOrDefaultAsync(cl => cl.Id == clienteId);
            // Do algorithm for sugeridos
            if(cliente.IdEstadoCivilNavigation.Id == 1){
                // Está soltero.
                var sugeridoResponse = await ObtenerSugerido(80, cliente, solicitud);
                var solicitudResponse = await Db.Solicitudes.AddAsync(sugeridoResponse.solicitud);
                await Db.SaveChangesAsync();
                var solicitudFromDb = await this.Db.Solicitudes
                                                .Include(sl => sl.IdClienteNavigation)
                                                .Include(sl => sl.IdTarjetaCreditoNavigation)
                                                .FirstOrDefaultAsync(sl => sl.Id == solicitudResponse.Entity.Id);
                return Ok(solicitudFromDb);
            }else if (cliente.IdEstadoCivilNavigation.Id == 2 && cliente.Hijos == 0){
                var sugeridoResponse = await ObtenerSugerido(70, cliente, solicitud);
                var solicitudResponse = await Db.Solicitudes.AddAsync(sugeridoResponse.solicitud);
                await Db.SaveChangesAsync();
                var solicitudFromDb = await this.Db.Solicitudes
                                                .Include(sl => sl.IdClienteNavigation)
                                                .Include(sl => sl.IdTarjetaCreditoNavigation)
                                                .FirstOrDefaultAsync(sl => sl.Id == solicitudResponse.Entity.Id);
                return Ok(solicitudFromDb);
            }else if (cliente.IdEstadoCivilNavigation.Id == 2 && cliente.Hijos == 1){
                var sugeridoResponse = await ObtenerSugerido(60, cliente, solicitud);
                var solicitudResponse = await Db.Solicitudes.AddAsync(sugeridoResponse.solicitud);
                await Db.SaveChangesAsync();
                var solicitudFromDb = await this.Db.Solicitudes
                                                .Include(sl => sl.IdClienteNavigation)
                                                .Include(sl => sl.IdTarjetaCreditoNavigation)
                                                .FirstOrDefaultAsync(sl => sl.Id == solicitudResponse.Entity.Id);
                return Ok(solicitudFromDb);

            }else if (cliente.IdEstadoCivilNavigation.Id == 2 && cliente.Hijos == 2){
                var sugeridoResponse = await ObtenerSugerido(55, cliente, solicitud);
                var solicitudResponse = await Db.Solicitudes.AddAsync(sugeridoResponse.solicitud);
                await Db.SaveChangesAsync();
                var solicitudFromDb = await this.Db.Solicitudes
                                                .Include(sl => sl.IdClienteNavigation)
                                                .Include(sl => sl.IdTarjetaCreditoNavigation)
                                                .FirstOrDefaultAsync(sl => sl.Id == solicitudResponse.Entity.Id);
                return Ok(solicitudFromDb);

            }else if (cliente.IdEstadoCivilNavigation.Id == 2 && cliente.Hijos >= 3){
                var sugeridoResponse = await ObtenerSugerido(50, cliente, solicitud);
                var solicitudResponse = await Db.Solicitudes.AddAsync(sugeridoResponse.solicitud);
                await Db.SaveChangesAsync();
                var solicitudFromDb = await this.Db.Solicitudes
                                                .Include(sl => sl.IdClienteNavigation)
                                                .Include(sl => sl.IdTarjetaCreditoNavigation)
                                                .FirstOrDefaultAsync(sl => sl.Id == solicitudResponse.Entity.Id);
                return Ok(solicitudFromDb);
            }

            return NotFound();

        }

        [HttpPost]
        [Route("nuevo")]
        public async Task<ActionResult<Cliente>> NuevoCliente([FromBody]Cliente cliente){
            var clientExists = await this.Db.Clientes.FirstOrDefaultAsync(cl => 
                    cl.Curp == cliente.Curp
                );
            if(clientExists != null) return BadRequest(new {
                msg = $"La CURP {clientExists.Curp} ya existe en el sistema."
            });
            var result = await this.Db.AddAsync(cliente);
            await this.Db.SaveChangesAsync();
            return Ok(result.Entity);
        }

        public (decimal Total, decimal Mensualidad) CalculateEjecution(int meses, decimal limite){
            var tasa = Convert.ToDecimal( this.GetTasa(meses) );
            decimal totally = limite + (limite * tasa / 100);
            decimal mensuality = totally / meses;
            return ( Math.Round(totally, 2), Math.Round(mensuality, 2) );
        }

        [HttpPost]
        [Route("prueba/{curp}")]
        public async Task<ActionResult<object>> EjecutarPrueba([FromRoute]string curp, [FromBody]EjecucionData data){
            var cliente = await this.Db.Clientes.FirstOrDefaultAsync(cl => 
                cl.Curp == curp
            );
            var tarjeta = await this.Db.TarjetasCreditos.FirstOrDefaultAsync(tar => 
                tar.Id == data.TarjetaId
            );

            if(tarjeta == null){
                return BadRequest(new {
                    msg = $"No se encontro la tarjeta solicitada."
                });
            }

            if(cliente == null){
                return BadRequest(new {
                    msg = $"No se encontro la curp {curp} en el sistema."
                });
            }

            // Logica de ejecucion.
            
            var (Total, Mensualidad) = CalculateEjecution(data.Meses, tarjeta.LimiteCredito??0);
            return Ok(new {
                Total,
                Mensualidad
            });
        }

    }
}