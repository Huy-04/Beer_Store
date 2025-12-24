using Domain.Core.Base;

namespace BeerStore.Domain.Entities.Auth.Junction
{
    public class UserAddress : Entity
    {
        public Guid UserId { get; private set; }
        public Guid AddressId { get; private set; }

        private UserAddress()
        { }

        private UserAddress(Guid id, Guid userId, Guid addressId)
            : base(id)
        {
            UserId = userId;
            AddressId = addressId;
        }

        public static UserAddress Create(Guid userId, Guid addressId)
        {
            var userAddress = new UserAddress(Guid.NewGuid(), userId, addressId);
            return userAddress;
        }
    }
}