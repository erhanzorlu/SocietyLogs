using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocietyLogs.Application.Features.Communities.Commands.CreateCommunity;
using SocietyLogs.Application.Features.Communities.Queries.GetAllCommunities;
using System.Security.Claims;

namespace SocietyLogs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommunitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize] // Sadece giriş yapmış üyeler
        public async Task<IActionResult> Create([FromForm] CreateCommunityCommand command)
        {

            // Token'dan User ID'yi çekip Command'e ekliyoruz
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
                command.CreatorUserId = Guid.Parse(userId);

            // İsteği gönder
            var response = await _mediator.Send(command);

            // Sonuç Başarılıysa 200 OK, değilse 400 Bad Request
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCommunitiesQuery query)
        {
            // [FromQuery] dememizin sebebi: URL'den ?PageNumber=1&PageSize=5 şeklinde gelecek.
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
