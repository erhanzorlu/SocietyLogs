using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocietyLogs.Application.Features.Companies.Commands.Create;

namespace SocietyLogs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompaniesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyCommand command)
        {
            // İsteği al -> MediatR'a fırlat -> Sonucu (Guid) dön
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
