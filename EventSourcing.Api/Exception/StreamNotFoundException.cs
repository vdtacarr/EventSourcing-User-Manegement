using System;
namespace EventSourcing.Api.Exception

{
    public class StreamNotFoundException : IOException
    {
        public StreamNotFoundException() : base("Stream not found")
        { }
        public StreamNotFoundException(string message) : base(message)
        { }
    }
}
