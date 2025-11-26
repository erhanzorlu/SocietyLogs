using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocietyLogs.Application.Features.Companies.Commands.Create;
using SocietyLogs.Application.Features.Companies.Commands.DeleteCompany;
using SocietyLogs.Application.Features.Companies.Commands.UpdateCompany;
using SocietyLogs.Application.Features.Companies.Queries.GetAllCompanies;
using SocietyLogs.Application.Features.Companies.Queries.GetCompanyById;

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

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        // READ ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllCompaniesQuery());
            return Ok(result);
        }

        // READ BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCompanyByIdQuery(id));
            if (result == null) return NotFound("Firma bulunamadı.");
            return Ok(result);
        }

        // UPDATE
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCompanyCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result) return NotFound("Güncellenecek firma bulunamadı.");
            return Ok("Güncelleme başarılı.");
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteCompanyCommand(id));
            if (!result) return NotFound("Silinecek firma bulunamadı.");
            return Ok("Firma silindi.");
        }
    }
}
