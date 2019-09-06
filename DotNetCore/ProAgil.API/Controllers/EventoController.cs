using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.API.Dtos;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository repo;
        private IMapper mapper;

        public EventoController(IProAgilRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await this.repo.GetAllEventoAsync(true);
                var results = this.mapper.Map<IEnumerable<EventoDto>>(eventos);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                return Ok();

                if (file.Length > 0) {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, fileName.Replace("\"", " ").Trim());

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                }

            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest("Erro ao realizar upload");

        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                var results = await this.repo.GetEventoAsyncById(eventoId, true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getByTema{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                var results = await this.repo.GetAllEventoAsyncByTema(tema, true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = this.mapper.Map<Evento>(model);
                this.repo.Add(evento);
                if (await this.repo.SaveChangesAsync()) {
                    return Created($"/api/evento/{model.Id}", this.mapper.Map<EventoDto>(model));
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }

        [HttpPut("{eventoid}")]
        public async Task<IActionResult> Put(int eventoId,EventoDto model)
        {
            try
            {

                var evento = await this.repo.GetEventoAsyncById(eventoId, false);
                if(evento == null)
                {
                    return NotFound();
                }

                this.mapper.Map(model, evento);

                this.repo.Update(model);

                if(await this.repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", this.mapper.Map<EventoDto>(model));
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }

        [HttpDelete("{eventoid}")]
        public async Task<IActionResult> Delete(int eventoId)
        {
            try
            {
                var evento = await this.repo.GetEventoAsyncById(eventoId, false);
                if(evento == null)
                {
                    return NotFound();
                }

                this.repo.Delete(evento);

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