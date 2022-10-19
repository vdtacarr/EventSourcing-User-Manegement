namespace EventSourcing.Api.Events
{
    public class UserCreated : IEvent
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailApprove { get; set; }
    }
}
