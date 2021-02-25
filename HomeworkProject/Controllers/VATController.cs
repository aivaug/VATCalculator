using Microsoft.AspNetCore.Mvc;
using Services.VATServices;
using System;
using System.Threading.Tasks;

namespace HomeworkProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VATController : ControllerBase
    {
        private readonly IVATService _VATService;
        public VATController(IVATService VATSservice)
        {
            _VATService = VATSservice;
        }

        [HttpGet("{providerID}/{clientID}/{price}")]
        public async Task<IActionResult> GetFullPrice(int providerID, int clientID, decimal price)
        {
            if (providerID < 1 || clientID < 1) return BadRequest("Please specify valid members properties");
            if (price < 0) return BadRequest("Please specify valid price");
            if (price == 0) return Ok(0);

            try
            {
                return Ok(await _VATService.GetVATFullPrice(providerID, clientID, price));
            } 
            catch(ArgumentException e)
            {
                return NotFound(e.Message);
            }
            catch(Exception e)
            {
                return BadRequest($"Error occured: {e.Message}");
            }
        }
    }
}
