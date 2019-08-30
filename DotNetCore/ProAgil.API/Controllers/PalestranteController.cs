using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.API.Controllers
{
    public class PalestranteController: ControllerBase
    {
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController: ControllerBase
    {
        private readonly IProAgilRepository repo;

        public EventoController(IProAgilRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet("{palestranteNome}")]
        public async Task<IActionResult> Get(string palestranteNome)
        {
            try
            {
                var results = await this.repo.GetAllPalestranteAsyncByNome(palestranteNome, true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("{palestranteId}")]
        public async Task<IActionResult> Get(int palestranteId)
        {
            try
            {
                var results = await this.repo.GetPalestranteAsyncById(palestranteId, true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Palestrante model)
        {
            try
            {
                this.repo.Add(model);
                if(await this.repo.SaveChangesAsync()){
                    return Created($"/api/palestrante/{model.Id}", model);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Put(int palestranteId, Palestrante model)
        {
            try
            {
                var evento = await this.repo.GetEventoAsyncById(palestranteId, false);
                if(evento == null)
                {
                    return NotFound();
                }

                this.repo.Add(model);

                if(await this.repo.SaveChangesAsync())
                {
                    return Created($"/api/palestrante/{model.Id}", model);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int palestranteId)
        {
            try
            {
                var palestrante = await this.repo.GetEventoAsyncById(palestranteId, false);
                if(palestrante == null)
                {
                    return NotFound();
                }

                this.repo.Delete(palestrante);

                if(await this.repo.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }


    }
    }
}