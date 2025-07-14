using MediatR;
using Microsoft.EntityFrameworkCore;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Aggregates;
using IdentityService.Domain.SeedWork;

namespace IdentityService.Infrastructure.Context
{
    public class IdentityDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public IdentityDbContext() : base() { }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options, IMediator mediator)
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
