using Mapster;
using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;

namespace SocietyLogs.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            // 1. Mapster ile DTO -> Entity dönüşümü
            // Property isimleri aynı olduğu için (CategoryName -> CategoryName) otomatik eşleşir.
            var category = request.Adapt<Category>();

            // 2. Generic Repo ile Ekleme
            // Bak! CategoryRepository diye bir class açmadık. Generic yapı halletti.
            await _unitOfWork.Repository<Category>().AddAsync(category, cancellationToken);

            // 3. Kaydet
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return category.Id;
        }
    }
}