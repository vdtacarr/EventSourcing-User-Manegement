﻿namespace EventSourcing.Api.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailApprove { get; set; }
    }
}
