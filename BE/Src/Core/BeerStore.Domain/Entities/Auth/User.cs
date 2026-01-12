using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Domain.ValueObjects.Auth.User;
using BeerStore.Domain.ValueObjects.Auth.User.Status;
using Domain.Core.Enums;

namespace BeerStore.Domain.Entities.Auth
{
    public class User : AggregateRoot
    {
        public Email Email { get; private set; }

        public Phone Phone { get; private set; }

        public FullName FullName { get; private set; }

        public UserName UserName { get; private set; }

        public Password Password { get; private set; }

        public UserStatus UserStatus { get; private set; }

        public EmailStatus EmailStatus { get; private set; }

        public PhoneStatus PhoneStatus { get; private set; }

        private readonly List<UserRole> _userRoles = new List<UserRole>();

        public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

        public Guid CreatedBy { get; private set; }

        public Guid UpdatedBy { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        private User()
        {
        }

        private User(Guid id, Email email, Phone phone, FullName fullName, UserName userName, Password password, UserStatus userStatus, EmailStatus emailStatus, PhoneStatus phoneStatus, Guid createdBy, Guid updatedBy)
            : base(id)
        {
            Email = email;
            Phone = phone;
            FullName = fullName;
            UserName = userName;
            Password = password;
            UserStatus = userStatus;
            EmailStatus = emailStatus;
            PhoneStatus = phoneStatus;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            CreatedAt = UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static User Create(Email email, Phone phone, FullName fullName, UserName userName, Password password, Guid createdBy, Guid updatedBy)
        {
            var user = new User(Guid.NewGuid(), email, phone, fullName, userName, password, UserStatus.Create(StatusEnum.Active), EmailStatus.Create(StatusEnum.Inactive), PhoneStatus.Create(StatusEnum.Inactive), createdBy, updatedBy);
            return user;
        }

        public void UpdateEmail(Email email)
        {
            if (Email == email) return;
            Email = email;
            Touch();
        }

        public void UpdateFullName(FullName fullName)
        {
            if (FullName == fullName) return;
            FullName = fullName;
            Touch();
        }

        public void UpdateUserName(UserName userName)
        {
            if (UserName == userName) return;
            UserName = userName;
            Touch();
        }

        public void UpdatePhone(Phone phone)
        {
            if (Phone == phone) return;
            Phone = phone;
            Touch();
        }

        public void UpdatePassword(Password password)
        {
            if (Password == password) return;
            Password = password;
            Touch();
        }

        public void UpdateUserStatus(UserStatus status)
        {
            if (UserStatus == status) return;
            UserStatus = status;
            Touch();
        }

        public void UserMaskAsActive(UserStatus status)
        {
            UpdateUserStatus(UserStatus.Create(StatusEnum.Active));
            Touch();
        }

        public void UserMaskAsInactive(UserStatus status)
        {
            UpdateEmailStatus(EmailStatus.Create(StatusEnum.Inactive));
            Touch();
        }

        public void UpdateEmailStatus(EmailStatus status)
        {
            if (EmailStatus == status) return;
            EmailStatus = status;
            Touch();
        }

        public void EmailMaskAsActive()
        {
            UpdateEmailStatus(EmailStatus.Create(StatusEnum.Active));
            Touch();
        }

        public void EmailMaskAsInactive()
        {
            UpdateEmailStatus(EmailStatus.Create(StatusEnum.Inactive));
            Touch();
        }

        public void UpdatePhoneStatus(PhoneStatus status)
        {
            if (PhoneStatus == status) return;
            PhoneStatus = status;
            Touch();
        }

        public void PhoneMaskAsActive()
        {
            UpdatePhoneStatus(PhoneStatus.Create(StatusEnum.Active));
            Touch();
        }

        public void PhoneMaskAsInactive()
        {
            UpdatePhoneStatus(PhoneStatus.Create(StatusEnum.Inactive));
            Touch();
        }

        public void SetUpdatedBy(Guid updateBy)
        {
            if (UpdatedBy == updateBy) return;
            UpdatedBy = updateBy;
            Touch();
        }

        // Role Management
        public UserRole AddRole(Guid userId, Guid roleId)
        {
            var userRole = UserRole.Create(userId, roleId);
            _userRoles.Add(userRole);
            Touch();
            return userRole;
        }

        public void RemoveUserRole(Guid idUserRole)
        {
            var userRole = _userRoles.FirstOrDefault(ur => ur.Id == idUserRole);
            if (userRole == null) return;
            _userRoles.Remove(userRole);
            Touch();
        }

        // Extension
        public void Touch()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
