using BeerStore.Domain.Enums.Auth;
using Domain.Core.Base;

namespace BeerStore.Domain.Entities.Auth.Junction
{
    public class UserCredential : Entity
    {
        public Guid UserId { get; private set; }
        public CredentialType CredentialType { get; private set; }
        public string ProviderKey { get; private set; }

        private UserCredential()
        { }

        private UserCredential(Guid id, Guid userId, CredentialType type, string key) : base(id)
        {
            UserId = userId;
            CredentialType = type;
            ProviderKey = key;
        }

        public static UserCredential Create(Guid userId, CredentialType type, string key)
        {
            return new UserCredential(Guid.NewGuid(), userId, type, key);
        }
    }
}