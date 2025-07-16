using MediatR;
using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.SeedWork;

namespace NotificationService.Infrastructure.Context
{
    public class NotificationDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public NotificationDbContext() : base() { }

        public NotificationDbContext(DbContextOptions<NotificationDbContext> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync(cancellationToken);

            var domainEntities = ChangeTracker
             .Entries<BaseEntity>()
             .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Any())
             .ToList();

            var domainEvents = domainEntities
                .SelectMany(e => e.Entity.DomainEvents)
                .ToList();

            domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await _mediator.Publish(domainEvent);

            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new UserEntityConfigurations());
        }
    }
}
