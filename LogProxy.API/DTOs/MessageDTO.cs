using System;

namespace LogProxy.API.DTOs
{
    public class MessageDTO
    {
        public string Id { get; set; }
        public string Summary { get; set; }
        public string Message { get; set; }
        public DateTime ReceivedAt { get; set; }
    }
}