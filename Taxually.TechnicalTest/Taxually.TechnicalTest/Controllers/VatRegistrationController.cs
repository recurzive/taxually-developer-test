using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Requests;
using Taxually.TechnicalTest.Services.Vat;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Taxually.TechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VatRegistrationController : ControllerBase
    {
        private const string CountryNotSupportedMessage = "Country not supported";
        private ITaxuallyVatService _vatService;

        public VatRegistrationController(ITaxuallyVatService vatService) {
            _vatService = vatService;
        }

        /// <summary>
        /// Registers a company for a VAT number in a given country
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] VatRegistrationRequest request)
        {
            switch (request.Country)
            {
                case "GB":
                    // UK has an API to register for a VAT number
                    await _vatService.RegisterVatUsingHttpClient(request);
                    break;
                case "FR":
                    // France requires an excel spreadsheet to be uploaded to register for a VAT number
                    await _vatService.RegisterVatUsingQueueClient(request);
                    break;
                case "DE":
                    // Germany requires an XML document to be uploaded to register for a VAT number
                    await _vatService.RegisterVatUsingXmlClient(request);
                    break;
                default:
                    throw new NotSupportedException(CountryNotSupportedMessage);
            }
            return Ok();
        }
    }
}
