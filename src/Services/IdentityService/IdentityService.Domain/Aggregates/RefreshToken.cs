using IdentityService.Domain.SeedWork;

namespace IdentityService.Domain.Aggregates
{
    public class RefreshToken : BaseEntity, IAggregateRoot
    {
        public Guid UserId { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public string CreatedByIp { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string? ReplacedByToken { get; private set; }

        protected RefreshToken() { }

        public RefreshToken(Guid userId, string token, DateTime expiresAt, string createdByIp)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
            CreatedByIp = createdByIp;
        }

        public void Revoke(string replacedByToken)
        {
            RevokedAt = DateTime.UtcNow;
            ReplacedByToken = replacedByToken;
        }

        public bool IsActive => RevokedAt == null && !IsExpired;
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    }
}
