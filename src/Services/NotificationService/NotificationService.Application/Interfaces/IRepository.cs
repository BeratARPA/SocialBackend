namespace NotificationService.Application.Interfaces
{
    public interface IRepository<T>
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
