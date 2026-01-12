using BeerStore.Application.DTOs.Auth.Role.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.RoleMap;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Roles.Commands.CreateRole
{
    public class CreateRoleCHandler : IRequestHandler<CreateRoleCommand, RoleResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<CreateRoleCHandler> _logger;

        public CreateRoleCHandler(IAuthUnitOfWork auow, ILogger<CreateRoleCHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<RoleResponse> Handle(CreateRoleCommand command, CancellationToken token)
        {
            await _auow.BeginTransactionAsync(token);

            try
            {
                var role = command.Request.ToRole(command.CreatedBy, command.UpdatedBy);

                if (await _auow.RRoleRepository.ExistsByNameAsync(role.RoleName, token))
                {
                    _logger.LogWarning("Role with Name {Name} already exists", role.RoleName.Value);
                    throw new BusinessRuleException<RoleField>(
                        ErrorCategory.Conflict,
                        RoleField.RoleName,
                        ErrorCode.NameAlreadyExists,
                        new Dictionary<object, object> {
                            { ParamField.Value, role.RoleName.Value }
                        });
                }

                await _auow.WRoleRepository.AddAsync(role, token);
                await _auow.CommitTransactionAsync(token);

                return role.ToRoleResponse();
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to create Role. Request: {@Request}",
                    command.Request
                );
                throw;
            }
        }
    }
}
