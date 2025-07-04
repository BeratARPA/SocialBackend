namespace IdentityService.Application.Interfaces
{
    public interface IRepository<T>
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
