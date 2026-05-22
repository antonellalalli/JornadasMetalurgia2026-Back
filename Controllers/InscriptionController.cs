using Jornadas_Metalurgia_2026.Enum;
using Jornadas_Metalurgia_2026.Models.Inscription;
using Jornadas_Metalurgia_2026.Models.Inscription.DTO;
using Jornadas_Metalurgia_2026.Services;
using Jornadas_Metalurgia_2026.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Jornadas_Metalurgia_2026.Controllers
{
    [Route("api/JornadaMetalurgica")]
    [ApiController]
    public class InscriptionController : ControllerBase
    {

        private readonly InscriptionService _inscriptionService;
        public InscriptionController(InscriptionService inscriptionService)
        {
            _inscriptionService = inscriptionService;

        }

        [HttpGet("inscriptions")]
        [Authorize(Roles = ROLE.ADMIN)]
        public async Task<ActionResult<List<Inscription>>> GetAllInscriptions([FromQuery] string? type
            , [FromQuery] bool? isActive = true)
        {
            try
            {

                var inscriptions = await _inscriptionService.GetAll(type, isActive);
                return Ok(inscriptions);
            }
            catch (HttpResponseError ex)
            {
                return StatusCode(
                    (int)ex.StatusCode,
                    new HttpMessage(ex.Message)
                    );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError, new HttpMessage(ex.Message));
            }

        }




        [HttpPost("create")]
        [ProducesResponseType(typeof(Inscription), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status500InternalServerError)]
        async public Task<ActionResult<Inscription>> CreateInscription([FromForm] InscriptionCreateDTO inscription)
        {
            try
            {
                var createdInscription = await _inscriptionService.CreateOneInscription(inscription);
                return Created("Inscription", createdInscription);
            }
            catch (HttpResponseError ex)
            {
                return StatusCode(
                    (int)ex.StatusCode,
                    new HttpMessage(ex.Message)
                    );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError, new HttpMessage(ex.Message));
            }
        }

        [HttpDelete("inscriptions/{id}")]
        [Authorize(Roles = ROLE.ADMIN)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status404NotFound)]
        async public Task<ActionResult> DeleteInscription(string id)
        {
            try
            {
                await _inscriptionService.DeleteOneById(id);
                return NoContent();
            }
            catch (HttpResponseError ex)
            {
                return StatusCode(
                    (int)ex.StatusCode,
                    new HttpMessage(ex.Message)
                    );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError, new HttpMessage(ex.Message));
            }
        }

        
















    }
}
