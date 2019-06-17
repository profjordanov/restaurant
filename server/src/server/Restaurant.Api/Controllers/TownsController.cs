using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Api.Controllers._Base;
using Restaurant.Domain.Repositories;

namespace Restaurant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TownsController : ApiController
    {
        private readonly ITownRepository _townRepository;

        public TownsController(ITownRepository townRepository)
        {
            _townRepository = townRepository;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]Guid townId)
        {
            var result = _townRepository.GetById(townId);
            return Ok(result);
        }
    }
}