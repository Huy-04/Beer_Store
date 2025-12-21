using BeerStore.Application.DTOs.Auth.Role.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.RoleMap;
using BeerStore.Domain.Enums.Messages;
using BeerStore.Domain.ValueObjects.Auth.Roles;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Role.Commands.UpdateRole
{
    public class UpdateRoleCHandler : IRequestHandler<UpdateRoleCommand, RoleResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<UpdateRoleCHandler> _logger;

        public UpdateRoleCHandler(IAuthUnitOfWork auow, ILogger<UpdateRoleCHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<RoleResponse> Handle(UpdateRoleCommand command, CancellationToken token)
        {
            await _auow.BeginTransactionAsync(token);

            try
            {
                var role = await _auow.RRoleRepository.GetByIdAsync(command.IdRole, token);
                if (role == null)
                {
                    _logger.LogWarning("Update failed: Role with Id={Id} not found", command.IdRole);
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
                    _logger.LogWarning("Update failed: Role with Name '{Name}' already exists", command.Request.RoleName);
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
                    "Exception occurred while updating Role. Request: {@Request}",
                    command.Request
                );
                throw;
            }
        }
    }
}
