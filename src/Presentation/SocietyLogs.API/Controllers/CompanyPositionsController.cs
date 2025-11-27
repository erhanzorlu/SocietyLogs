using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocietyLogs.Application.Features.Company_Positions.Commands.CreateCompanyPosition;
using SocietyLogs.Application.Features.Company_Positions.Commands.DeleteCompanyPosition;
using SocietyLogs.Application.Features.Company_Positions.Commands.RestoreCompanyPosition;
using SocietyLogs.Application.Features.Company_Positions.Commands.UpdateCompanyPosition;
using SocietyLogs.Application.Features.Company_Positions.Queries.GetAdminCompanyPosition;
using SocietyLogs.Application.Features.Company_Positions.Queries.GetAllCompanyPositions;
using SocietyLogs.Application.Features.Company_Positions.Queries.GetCompanyPositionById;

namespace SocietyLogs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyPositionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyPositionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCompanyPositionCommand command)
        {
            var id = await _mediator.Send(command);

            // 201 Created döner
            return CreatedAtAction(nameof(Create), new { id = id }, id);
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCompanyPositionCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result) return NotFound("Şirket Pozisyonu bulunamadı.");

            return Ok("Güncelleme başarılı.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteCompanyPositionCommand(id);
            var result = await _mediator.Send(command);

            if (!result) return NotFound("Silinecek Şirket Pozisyonu bulunamadı.");

            return NoContent(); // 204 No Content (Silme işlemi standardı)
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllCompanyPositionsQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetCompanyPositionByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound("Şirket Pozisyonu bulunamadı.");

            return Ok(result);
        }

        [HttpGet("admin/all")] //DENEME AMAÇLI SİLİNECEK
        public async Task<IActionResult> GetAllForAdmin()
        {
            // Bu sorgu ignoreQueryFilters: true ile çalışır
            return Ok(await _mediator.Send(new GetAdminComapnyPositionQuery()));
        }

        // 7. ADMIN TEST: Silineni Geri Getir (Restore) 🚑
        [HttpPut("restore/{id}")] //DENEME AMAÇLI SİLİNECEK
        public async Task<IActionResult> Restore(Guid id)
        {
            var result = await _mediator.Send(new RestoreCompanyPositionCommand(id));

            if (!result) return NotFound("Silinen Şirket Pozisyonu bulunamadı (veya ID yanlış).");

            return Ok("Şirket Pozisyonu başarıyla geri getirildi. Artık aktif.");
        }
    }
}
