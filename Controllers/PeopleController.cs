﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeopleWebApi.Interfaces;
using PeopleWebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace PeopleWebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IDBRepository<People, People> _dataRepository;
        private readonly IWebHostEnvironment _env;

        public PeopleController(IDBRepository<People, People> dataRepository, IWebHostEnvironment env)
        {
            _dataRepository = dataRepository;
            _env = env;
        }

        // GET: api/People
        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Возвращает реестр граждан", Tags = new[] { "Реестр граждан" })]
        [ProducesResponseType(typeof(IEnumerable<People>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<People>>> GetAsync()
        {
            var list = await _dataRepository.GetAllAsync(predicates: null,
                p => p.PeopleAdreses.Select(a => a.AdresType),
                p => p.PeopleDocuments);
            return Ok(list);
        }

        // GET: api/People/2
        [HttpGet("{id}", Name = "GetPeople")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Возвращает гражданина по идентификатору", Description = "Требуется Id", Tags = new[] { "Реестр граждан" })]
        [ProducesResponseType(typeof(People), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<People>> GetAsync(int id)
        {
            var item = await _dataRepository.GetDtoAsync(id);
            if (item == null)
                return NotFound("Объект не найден в базе данных.");
            return Ok(item);
        }

        // POST: api/People
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "1,2,3")]
        [SwaggerOperation(Summary = "Добавляет нового гражданина", Description = "Требуется описание", Tags = new[] { "Реестр граждан" })]
        [Consumes(MediaTypeNames.Application.Json)]//формат запроса
        [Produces(MediaTypeNames.Application.Json)]//фортам ответа
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostAsync([FromBody] People People)
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "1,2,3")]
        [SwaggerOperation(Summary = "Изменяет информацию по гражданину", Description = "Требуется описание и Id", Tags = new[] { "Реестр граждан" })]
        [Consumes(MediaTypeNames.Application.Json)]//формат запроса
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PutAsync(int id, [FromBody] People People)
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "1,2,3")]
        [SwaggerOperation(Summary = "Удаляет гражданина", Description = "Требуется Id", Tags = new[] { "Реестр граждан" })]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var it = await _dataRepository.GetAsync(id);
            if (it == null)
                return NotFound("Объект не найден в базе данных.");
            await _dataRepository.DeleteAsync(it);
            return NoContent();
        }

        // POST: api/People/pic
        [HttpPost]
        [Route("pictures")]
        [SwaggerOperation(Summary = "Загружает фотографию гражданина", Tags = new[] { "Реестр граждан" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    //var filePath = Path.GetTempFileName();
                    var filePath = Path.Combine(@"c:\Temp\", Path.GetRandomFileName());
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return Ok(new { count = files.Count, size });
        }

        // GET: api/People/2/pic
        [HttpGet]
        [Route("{id:int}/pictures")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "1,2,3")]
        [SwaggerOperation(Summary = "Возвращает фотографию гражданина", Description = "Требуется Id", Tags = new[] { "Реестр граждан" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetImage(int id)
        {
            if (id <= 0)
                return BadRequest();

            var webRoot = _env.WebRootPath;
            var path = @"c:\Temp\vor.jpg";
            if (System.IO.File.Exists(path))
            {
                string imageFileExtension = Path.GetExtension(path);
                string mimetype = GetImageMimeTypeFromImageFileExtension(imageFileExtension);
                var buffer = System.IO.File.ReadAllBytes(path);
                return File(buffer, mimetype);
            }                
            return NotFound();
        }

        private string GetImageMimeTypeFromImageFileExtension(string extension)
        {
            string mimetype;
            switch (extension)
            {
                case ".png":
                    mimetype = "image/png";
                    break;
                case ".gif":
                    mimetype = "image/gif";
                    break;
                case ".jpg":
                case ".jpeg":
                    mimetype = "image/jpeg";
                    break;
                case ".bmp":
                    mimetype = "image/bmp";
                    break;
                case ".tiff":
                    mimetype = "image/tiff";
                    break;
                case ".wmf":
                    mimetype = "image/wmf";
                    break;
                case ".jp2":
                    mimetype = "image/jp2";
                    break;
                case ".svg":
                    mimetype = "image/svg+xml";
                    break;
                default:
                    mimetype = "application/octet-stream";
                    break;
            }
            return mimetype;
        }

    }
}
