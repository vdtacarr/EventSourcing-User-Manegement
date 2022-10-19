namespace EventSourcing.Api.Events
{
    public class UserNameChanged : IEvent
    {
        public int UserId { get; set; }
        public string NewName { get; set; }
    }
}
