using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocietyLogs.Application.Features.Users.Commands.UploadProfileImage;
using System.Security.Claims;

namespace SocietyLogs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload-avatar")]
        // DİKKAT: [FromForm] ifadesini sildik. Sadece IFormFile file kaldı.
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized();

            var command = new UploadProfileImageCommand
            {
                ProfileImage = file,
                UserId = Guid.Parse(userIdString)
            };

            var result = await _mediator.Send(command);

            return Ok(new { ImageUrl = result });
        }
    }
}
