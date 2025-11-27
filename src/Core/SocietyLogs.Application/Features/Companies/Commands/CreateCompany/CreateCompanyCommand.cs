using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Companies.Commands.Create
{
    // MediatR'a diyoruz ki: "Bu komut çalıştığında geriye Guid (oluşan şirketin ID'si) dönecek."
    public record CreateCompanyCommand(
        string CompanyName,
        string? Description,
        string? LogoUrl
    ) : IRequest<Guid>;
}
