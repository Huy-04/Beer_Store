using BeerStore.Application.DTOs.Auth.Role.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.RoleMap;
using BeerStore.Domain.Enums.Auth.Messages;
using BeerStore.Domain.ValueObjects.Auth.Role;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Roles.Commands.UpdateRole
{
    public class UpdateRoleCHandler : IRequestHandler<UpdateRoleCommand, RoleResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<UpdateRoleCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public UpdateRoleCHandler(IAuthUnitOfWork auow, ILogger<UpdateRoleCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<RoleResponse> Handle(UpdateRoleCommand command, CancellationToken token)
        {
            _authService.EnsureCanUpdateRole();

            await _auow.BeginTransactionAsync(token);

            try
            {
                var role = await _auow.RRoleRepository.GetByIdAsync(command.IdRole, token);
                if (role == null)
                {
                    _logger.LogWarning("Role {Id} not found", command.IdRole);
                    throw new BusinessRuleException<RoleField>(
                        ErrorCategory.NotFound,
                        RoleField.IdRole,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            {ParamField.Value,command.IdRole }
                        });
                }

                var newName = RoleName.Create(command.Request.RoleName);

                if (await _auow.RRoleRepository.ExistsByNameAsync(newName, token, role.Id))
                {
                    _logger.LogWarning("Role with Name {Name} already exists", command.Request.RoleName);
                    throw new BusinessRuleException<RoleField>(
                        ErrorCategory.Conflict,
                        RoleField.RoleName,
                        ErrorCode.NameAlreadyExists,
                        new Dictionary<object, object> {
                            { ParamField.Value, newName.Value }
                        });
                }

                role.ApplyRole(command.Request, command.UpdatedBy);
                _auow.WRoleRepository.Update(role);
                await _auow.CommitTransactionAsync(token);

                return role.ToRoleResponse();
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to update Role. Request: {@Request}",
                    command.Request
                );
                throw;
            }
        }
    }
}

