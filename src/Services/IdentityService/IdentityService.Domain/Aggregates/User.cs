using IdentityService.Domain.SeedWork;

namespace IdentityService.Domain.Aggregates
{
    public class User : BaseEntity, IAggregateRoot
    {
        public string Email { get; private set; }
        public bool EmailConfirmed { get; private set; }

        public string PasswordHash { get; private set; }
        public string PasswordSalt { get; private set; }

        public string PhoneNumber { get; private set; }
        public bool PhoneNumberConfirmed { get; private set; }

        public bool TwoFactorEnabled { get; private set; }

        public bool LockoutEnabled { get; private set; }
        public DateTime? LockoutEnd { get; private set; }
        public int AccessFailedCount { get; private set; }

        public bool IsDeleted { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        protected User() { }

        public User(string email, string passwordHash, string passwordSalt)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            SetEmail(email);
            SetPassword(passwordHash, passwordSalt);

            EmailConfirmed = false;
            PhoneNumberConfirmed = false;
            TwoFactorEnabled = false;
            LockoutEnabled = true;
            AccessFailedCount = 0;

            //AddDomainEvent(new UserCreatedDomainEvent(Id, Email));
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            Email = email;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ConfirmEmail()
        {
            EmailConfirmed = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPassword(string passwordHash, string passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("PasswordHash cannot be empty.", nameof(passwordHash));

            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ConfirmPhoneNumber()
        {
            PhoneNumberConfirmed = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void EnableTwoFactor()
        {
            TwoFactorEnabled = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void LockOut(DateTime until)
        {
            LockoutEnd = until;
            UpdatedAt = DateTime.UtcNow;
        }

        public void IncreaseAccessFailedCount()
        {
            AccessFailedCount++;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ResetAccessFailedCount()
        {
            AccessFailedCount = 0;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
