using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocietyLogs.Application.Features.Users.Commands.UpdateProfile;
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
        public async Task<IActionResult> UploadAvatar([FromForm] UploadProfileImageCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.Success)
                return BadRequest(response); // Hata mesajı döner

            return Ok(response); // { Data: "/uploads/...", Success: true, ... }
        }

        // PUT: api/users/update-profile
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserProfileCommand command)
        {
            // 💡 NOT: UserId parametresini dışarıdan almıyoruz.
            // Handler içinde CurrentUserService (Token) üzerinden otomatik dolacak.

            // [FromForm] sayesinde Swagger'da dosya yükleme butonu çıkacak.
            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
