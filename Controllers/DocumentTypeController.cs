using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeopleWebApi.Interfaces;
using PeopleWebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace PeopleWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentTypeController : ControllerBase
    {
        private readonly IDBRepository<DocumentType, DocumentType> _dataRepository;

        public DocumentTypeController(IDBRepository<DocumentType, DocumentType> dataRepository) => _dataRepository = dataRepository;

        // GET: api/DocumentType
        [HttpGet]
        [SwaggerOperation(Summary = "Возвращает справочник типов документов", Tags = new[] { "Справочник типов документов" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var list = await _dataRepository.GetAllAsync();
            return Ok(list);
        }

        // GET: api/DocumentType/2
        [HttpGet("{id}", Name = "GetDocumentType")]
        [SwaggerOperation(Summary = "Возвращает тип документа", Description = "Требуется Id", Tags = new[] { "Справочник типов документов" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _dataRepository.GetDtoAsync(id);
            if (item == null)
                return NotFound("Объект не найден в базе данных.");

            return Ok(item);
        }

        // POST: api/DocumentType
        [HttpPost]
        [SwaggerOperation(Summary = "Добавляет тип документа", Description = "Требуется описание и Id", Tags = new[] { "Справочник типов документов" })]
        [Consumes(MediaTypeNames.Application.Json)]//формат запроса
        [Produces(MediaTypeNames.Application.Json)]//фортам ответа
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] DocumentType DocumentType)
        {
            if (DocumentType is null)
                return BadRequest("Объект пуст.");

            if (!ModelState.IsValid)
                return BadRequest("Объект не прошел валидацию.");

            await _dataRepository.AddAsync(DocumentType);
            return CreatedAtRoute("GetDocumentType", new { Id = DocumentType.Id }, null);
        }

        // PUT: api/DocumentType/3
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Изменяет тип документа", Description = "Требуется описание и Id", Tags = new[] { "Справочник типов документов" })]
        [Consumes(MediaTypeNames.Application.Json)]//формат запроса
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] DocumentType DocumentType)
        {
            if (DocumentType == null)
                return BadRequest("Объект пуст.");

            if (DocumentType.Id != id)
                return BadRequest("Объект ссылается на другой объект в базе данных");

            if (await _dataRepository.GetAsync(id) == null)
                return NotFound("Объект не найден в базе данных.");

            if (!ModelState.IsValid)
                return BadRequest("Объект не прошел валидацию.");

            await _dataRepository.UpdateAsync(DocumentType);
            return NoContent();
        }

        // DELETE: api/DocumentType/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Удаляет тип документа", Description = "Требуется Id", Tags = new[] { "Справочник типов документов" })]
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
