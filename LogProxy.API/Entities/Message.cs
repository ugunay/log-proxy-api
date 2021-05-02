using System;

namespace LogProxy.API.Entities
{
    public class Message
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime ReceivedAt { get; set; }
    }
}