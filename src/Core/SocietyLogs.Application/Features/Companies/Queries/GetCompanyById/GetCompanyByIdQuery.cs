using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Companies.Queries.GetCompanyById
{
    // Request
    public record GetCompanyByIdQuery(Guid Id) : IRequest<Company>;

    // Handler
    public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, Company>
    {
        private readonly IGenericRepository<Company> _repository;

        public GetCompanyByIdQueryHandler(IGenericRepository<Company> repository)
        {
            _repository = repository;
        }

        public async Task<Company> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id);
        }
    }
}
