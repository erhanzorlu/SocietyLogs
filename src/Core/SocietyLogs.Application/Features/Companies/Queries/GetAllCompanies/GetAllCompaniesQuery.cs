using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Companies.Queries.GetAllCompanies
{
    // Request
    public record GetAllCompaniesQuery : IRequest<List<Company>>;

    // Handler
    public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, List<Company>>
    {
        private readonly IGenericRepository<Company> _repository;

        public GetAllCompaniesQueryHandler(IGenericRepository<Company> repository)
        {
            _repository = repository;
        }

        public async Task<List<Company>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
