using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Reservas.Api.Interfaces;
using Reservas.Api.Models;

namespace Reservas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private IReservaRepository repository;

        public ReservasController(IReservaRepository repo) => repository = repo;

        [HttpGet]
        public IActionResult Get()
        {
            var reservas = repository.Reservas;
            return Ok(reservas);
        }

        [HttpGet("{ReservaId}")]
        public ActionResult<Reserva> Get(int ReservaId)
        {
            var reserva = repository[ReservaId];
            if (reserva == null)
                return NotFound();
            return Ok(reserva);
        }

        [HttpPost]
        public ActionResult<Reserva> Post([FromBody] Reserva res)
        {
            var novaReserva = repository.AddReserva(new Reserva
            {
                Nome = res.Nome,
                InicioLocacao = res.InicioLocacao,
                FimLocacao = res.FimLocacao
            });

            return CreatedAtAction(nameof(Get), new { ReservaId = novaReserva.ReservaId }, novaReserva);
        }

        [HttpPut]
        public ActionResult<Reserva> Put([FromBody] Reserva res)
        {
            var atualizada = repository.UpdateReserva(res);
            return Ok(atualizada);
        }

        [HttpPatch("{ReservaId}")]
        public IActionResult Patch(int ReservaId, [FromBody] JsonPatchDocument<Reserva> patch)
        {
            var res = repository[ReservaId];
            if (res == null)
                return NotFound();

            patch.ApplyTo(res);
            return Ok();
        }

        [HttpDelete("{ReservaId}")]
        public IActionResult Delete(int ReservaId)
        {
            repository.DeleteReserva(ReservaId);
            return NoContent();
        }
    }
}
