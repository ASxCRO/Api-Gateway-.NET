using AutoStoper.API.Data.Database.Models;
using AutoStoper.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AutoStoper.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VoznjaController : ControllerBase
    {
        private readonly IVoznjaService voznjaService;

        public VoznjaController(IVoznjaService voznjaService)
        {
            this.voznjaService = voznjaService;
        }

        [HttpGet]
        [Route("Get")]
        public IEnumerable<Voznja> Get()
        {
            return voznjaService.GetAll();
        }
    }
}
