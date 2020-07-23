﻿using System;
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
    public class AdresTypeController : ControllerBase
    {
        private readonly IDBRepository<AdresType, AdresType> _dataRepository;

        public AdresTypeController(IDBRepository<AdresType, AdresType> dataRepository) => _dataRepository = dataRepository;

        // GET: api/AdresType
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var list = await _dataRepository.GetAllAsync();
            return Ok(list);
        }

        // GET: api/AdresType/2
        [HttpGet("{id}", Name = "GetAdresType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _dataRepository.GetDtoAsync(id);
            if (item == null)
                return NotFound("Объект не найден в базе данных.");

            return Ok(item);
        }

        // POST: api/AdresType
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]//формат запроса
        [Produces(MediaTypeNames.Application.Json)]//фортам ответа
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] AdresType adresType)
        {
            if (adresType is null)
                return BadRequest("Объект пуст.");

            if (!ModelState.IsValid)
                return BadRequest("Объект не прошел валидацию.");

            await _dataRepository.AddAsync(adresType);
            return CreatedAtRoute("GetAdresType", new { Id = adresType.Id }, null);
        }

        // PUT: api/AdresType/3
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]//формат запроса
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] AdresType adresType)
        {
            if (adresType == null)
                return BadRequest("Объект пуст.");

            if (adresType.Id != id)
                return BadRequest("Объект ссылается на другой объект в базе данных");

            if (await _dataRepository.GetAsync(id) == null)
                return NotFound("Объект не найден в базе данных.");

            if (!ModelState.IsValid)
                return BadRequest("Объект не прошел валидацию.");

            await _dataRepository.UpdateAsync(adresType);
            return NoContent();
        }

        // DELETE: api/AdresType/5
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
