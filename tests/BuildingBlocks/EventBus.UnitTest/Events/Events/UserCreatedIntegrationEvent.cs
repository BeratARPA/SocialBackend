using EventBus.Base.Events;

namespace EventBus.UnitTest.Events.Events
{
    public class UserCreatedIntegrationEvent:IntegrationEvent
    {
        public Guid Id { get; set; }
        
        public UserCreatedIntegrationEvent(Guid id)
        {
            Id = id;
        }
    }
}
