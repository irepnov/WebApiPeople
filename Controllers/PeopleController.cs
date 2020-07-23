using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeopleWebApi.Interfaces;
using PeopleWebApi.Models;

namespace PeopleWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IDBRepository<People, People> _dataRepository;

        public PeopleController(IDBRepository<People, People> dataRepository) => _dataRepository = dataRepository;

        // GET: api/People
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var list = await _dataRepository.GetAllAsync(predicates: null,
                p => p.PeopleAdreses.Select(a => a.AdresType),
                p => p.PeopleDocuments);
            return Ok(list);
        }

        // GET: api/People/2
        [HttpGet("{id}", Name = "GetPeople")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _dataRepository.GetDtoAsync(id);
            if (item == null)
                return NotFound("Объект не найден в базе данных.");

            return Ok(item);
        }

        // POST: api/People
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]//формат запроса
        [Produces(MediaTypeNames.Application.Json)]//фортам ответа
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] People People)
        {
            if (People is null)
                return BadRequest("Объект пуст.");

            if (!ModelState.IsValid)
                return BadRequest("Объект не прошел валидацию.");

            await _dataRepository.AddAsync(People);
            return CreatedAtRoute("GetPeople", new { Id = People.Id }, null);
        }

        // PUT: api/People/3
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]//формат запроса
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] People People)
        {
            if (People == null)
                return BadRequest("Объект пуст.");

            if (People.Id != id)
                return BadRequest("Объект ссылается на другой объект в базе данных");

            if (await _dataRepository.GetAsync(id) == null)
                return NotFound("Объект не найден в базе данных.");

            if (!ModelState.IsValid)
                return BadRequest("Объект не прошел валидацию.");

            await _dataRepository.UpdateAsync(People);
            return NoContent();
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var it = await _dataRepository.GetAsync(id);
            if (it == null)
            {
                return NotFound("Объект не найден в базе данных.");
            }

            await _dataRepository.DeleteAsync(it);
            return NoContent();
        }

    }
}
