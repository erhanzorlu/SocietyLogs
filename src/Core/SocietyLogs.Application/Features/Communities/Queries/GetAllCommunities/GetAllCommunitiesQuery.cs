using MediatR;
using SocietyLogs.Application.Common.Wrappers;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Communities.Queries.GetAllCommunities
{
    // Geriye ServiceResponse içinde "Community Listesi" döneceğiz.
    public class GetAllCommunitiesQuery : IRequest<ServiceResponse<List<Community>>>
    {
        public int PageNumber { get; set; } = 1; // Varsayılan 1. sayfa
        public int PageSize { get; set; } = 10;  // Varsayılan 10 kayıt
    }
}
