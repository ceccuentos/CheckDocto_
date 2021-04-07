using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckDocto.Data;
using CheckDocto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheckDocto.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors("PermitirApiRequest")]

    public class CheckDoctoController : Controller
    {

        private readonly CheckDoctoRepository _repository;
        private readonly ILogger<CheckDoctoController> _logger;

        public CheckDoctoController(CheckDoctoRepository repository, ILogger<CheckDoctoController> Logger)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _logger = Logger;
        }

        /*        // GET: api/values
        [HttpGet]
        public ActionResult Get()
        {
            _logger.LogInformation("Llamada por Get");
            return ValidationProblem("Debe especificar el keyDocto");
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<List<KeyDocto>>> Get(double id)
        {
            try
            {
                _logger.LogInformation($"Llamada por Get con {1}");
                return Ok(new { KeyDoctos = await _repository.Check(id) });

            } catch (Exception ex)
            {
                _logger.LogWarning($"Error con id {id}");
                return BadRequest(ex.Message);
                //ValidationProblem(ex.Message); //StatusCode(500);
            }
        }
       */
        // POST Key
        /// <summary>
        /// Valida Documento específico
        /// </summary>
        /// <remarks>
        /// Ejemplo request:
        ///
        ///     POST /CheckDocto
        ///     {
        ///        "Empresa": "002",
        ///        "Tipodocto": "Nv Autoventa",
        ///        "Correlativo": 1254
        ///     }
        /// </remarks>
        /// <param name="Key"></param>
        /// <response code="200">Exitoso</response>
        /// <response code="400">Body Inválido</response>
        /// <response code="500">Error Interno</response>
        [HttpPost]
        public async Task<ActionResult<List<KeyDocto>>> Post([FromBody] KeyDocto Key)
        {
            try
            {
                _logger.LogInformation($"POST con id {Key.Correlativo}");
                return Ok(new { KeyDoctos = await _repository.CheckDocumento(Key) });
              

            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error en POST con id {Key.Correlativo}");
                return BadRequest(ex.Message);
            }
        }

    }
}
