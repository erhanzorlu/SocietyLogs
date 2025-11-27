using MediatR;
using Microsoft.AspNetCore.Mvc;
using SocietyLogs.Application.Features.Categories.Commands.CreateCategory;
using SocietyLogs.Application.Features.Categories.Commands.DeleteCategory;
using SocietyLogs.Application.Features.Categories.Commands.RestoreCategory;
using SocietyLogs.Application.Features.Categories.Commands.UpdateCategory;
using SocietyLogs.Application.Features.Categories.Queries.GetAdminCategories;
using SocietyLogs.Application.Features.Categories.Queries.GetAllCategories;
using SocietyLogs.Application.Features.Categories.Queries.GetCategoryById;

namespace SocietyLogs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            var id = await _mediator.Send(command);

            // 201 Created döner
            return CreatedAtAction(nameof(Create), new { id = id }, id);
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result) return NotFound("Kategori bulunamadı.");

            return Ok("Güncelleme başarılı.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteCategoryCommand(id);
            var result = await _mediator.Send(command);

            if (!result) return NotFound("Silinecek kategori bulunamadı.");

            return NoContent(); // 204 No Content (Silme işlemi standardı)
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllCategoriesQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetCategoryByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound("Kategori bulunamadı.");

            return Ok(result);
        }


        // 3. ADMIN TEST: Silinenler dahil her şeyi gör (Röntgen Modu) 👀
        [HttpGet("admin/all")] //DENEME AMAÇLI SİLİNECEK
        public async Task<IActionResult> GetAllForAdmin()
        {
            // Bu sorgu ignoreQueryFilters: true ile çalışır
            return Ok(await _mediator.Send(new GetAdminCategoriesQuery()));
        }

        // 7. ADMIN TEST: Silineni Geri Getir (Restore) 🚑
        [HttpPut("restore/{id}")] //DENEME AMAÇLI SİLİNECEK
        public async Task<IActionResult> Restore(Guid id)
        {
            var result = await _mediator.Send(new RestoreCategoryCommand(id));

            if (!result) return NotFound("Silinen kategori bulunamadı (veya ID yanlış).");

            return Ok("Kategori başarıyla geri getirildi. Artık aktif.");
        }
    }
}
