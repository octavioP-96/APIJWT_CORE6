using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.HomeModels;
using Services.HomeServices;

namespace BasePlantilla.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller {
        private readonly IHomeService _service;
        public HomeController(IHomeService service) {
            _service = service;
        }

        [HttpGet]
        public ActionResult Index() {
            return Ok("Plantilla API");
        }

        // POST: HomeController/Create
        [HttpPost("Saludar")]
        public ActionResult Saludar(Person persona) {
            _service.SetPerson(persona);
            return Ok(_service.Saludar());
        }

    }
}
