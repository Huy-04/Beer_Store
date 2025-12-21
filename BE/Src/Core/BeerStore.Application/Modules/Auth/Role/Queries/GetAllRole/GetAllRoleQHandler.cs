using BeerStore.Application.DTOs.Auth.Role.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.RoleMap;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Role.Queries.GetAllRole
{
    public class GetAllRoleQHandler : IRequestHandler<GetAllRoleQuery, IEnumerable<RoleResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetAllRoleQHandler> _logger;

        public GetAllRoleQHandler(IAuthUnitOfWork auow, ILogger<GetAllRoleQHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<IEnumerable<RoleResponse>> Handle(GetAllRoleQuery query, CancellationToken token)
        {
            var list = await _auow.RRoleRepository.GetAllAsync(token);
            return list.Select(r => r.ToRoleResponse());
        }
    }
}