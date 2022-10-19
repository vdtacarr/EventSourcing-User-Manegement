namespace EventSourcing.Api.Events
{
    public class UserEmailApproved : IEvent
    {
        public int UserId { get; set; }
    }
}
