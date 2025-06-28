using UserService.Domain.SeedWork;

namespace UserService.Domain.Aggregates
{
    public class User : BaseEntity, IAggregateRoot
    {
        public string Username { get; private set; }

        protected User() { }

        public User(string username) : this()
        {
            Username = username;
        }

        public void SetUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));

            Username = username;
        }
    }
}
