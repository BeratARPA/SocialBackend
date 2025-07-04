using UserService.Domain.SeedWork;

namespace UserService.Domain.Aggregates
{
    public class User : BaseEntity, IAggregateRoot
    {
        public string Username { get; private set; }
        public string DisplayName { get; private set; }
        public string Bio { get; private set; }
        public string ProfileImageUrl { get; private set; }
        public string BannerImageUrl { get; private set; }
        public DateTime? BirthDate { get; private set; }
        public string Location { get; private set; }
        public string WebsiteUrl { get; private set; }
        public bool IsPrivate { get; private set; }
        public int FollowersCount { get; private set; }
        public int FollowingCount { get; private set; }
        public int PostsCount { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        protected User() { }

        public User(string username, string displayName)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            Username = username;
            DisplayName = displayName;
            Bio = string.Empty;
            ProfileImageUrl = string.Empty;
            BannerImageUrl = string.Empty;

            FollowersCount = 0;
            FollowingCount = 0;
            PostsCount = 0;

            IsPrivate = false;
            IsDeleted = false;
        }

        public void UpdateProfile(string displayName, string bio, string location, string websiteUrl)
        {
            DisplayName = displayName;
            Bio = bio;
            Location = location;
            WebsiteUrl = websiteUrl;

            UpdatedAt = DateTime.UtcNow;
        }

        public void SetProfileImage(string imageUrl)
        {
            ProfileImageUrl = imageUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetBannerImage(string bannerUrl)
        {
            BannerImageUrl = bannerUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPrivacy(bool isPrivate)
        {
            IsPrivate = isPrivate;
            UpdatedAt = DateTime.UtcNow;
        }

        public void IncrementFollowers() => FollowersCount++;
        public void DecrementFollowers() => FollowersCount--;

        public void IncrementFollowing() => FollowingCount++;
        public void DecrementFollowing() => FollowingCount--;

        public void IncrementPosts() => PostsCount++;
        public void DecrementPosts() => PostsCount--;

        public void SoftDelete()
        {
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
