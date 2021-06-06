using ApiGateway.Core.Models.RequestModels;
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

        [HttpPost]
        [Route("Insert")]
        public IActionResult Insert(Voznja voznja)
        {
            voznjaService.Insert(voznja);
            return Ok();
        }


        [HttpPost]
        [Route("InsertPutnika")]
        public IActionResult InsertPutnika(PrijavaNaVoznjuRequest prijavaNaVoznjuRequest)
        {
            voznjaService.InsertPutnika(prijavaNaVoznjuRequest);
            return Ok();
        }

        [HttpPost]
        [Route("DeletePutnika")]
        public IActionResult DeletePutnika(PrijavaNaVoznjuRequest prijavaNaVoznjuRequest)
        {
            voznjaService.DeletePutnika(prijavaNaVoznjuRequest);
            return Ok();
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<Voznja> Get()
        {
            return voznjaService.GetAll();
        }

        [HttpGet]
        [Route("GetById")]
        public Voznja GetById(int id)
        {
            return voznjaService.GetById(id);
        }

        [HttpGet]
        [Route("GetByUserId")]
        public List<Voznja> GetByUserId(int userId)
        {
            return voznjaService.GetByUserId(userId);
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(Voznja voznja)
        {
            voznjaService.Delete(voznja);
            return Ok();
        }

        [HttpPost]
        [Route("Update")]
        public IActionResult Update(Voznja voznja)
        {
            voznjaService.Update(voznja);
            return Ok();
        }
    }
}
