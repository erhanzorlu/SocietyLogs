using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Companies.Commands.Create
{
    // Bu komut çalışınca geriye Guid (oluşan ID) dönecek.
    public record CreateCompanyCommand(string CompanyName, string? Description) : IRequest<Guid>;
}
