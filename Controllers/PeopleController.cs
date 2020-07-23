using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> Get()
        {
            var list = await _dataRepository.GetAllAsync(predicates: null,
                p => p.PeopleAdreses.Select(a => a.AdresType),
                p => p.PeopleDocuments);
            return Ok(list);
        }

        // GET: api/People/2
        [HttpGet("{id}", Name = "GetPeople")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _dataRepository.GetDtoAsync(id);
            if (item == null)
                return NotFound("Объект не найден в базе данных.");

            return Ok(item);
        }

    }
}
