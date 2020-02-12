using System;
namespace Chat.Models
{
    public class ChatMessage
    {
        public Int64 key { get; set; }
        public Int64 toId { get; set; }
        public User to { get; set; }
        public Int64 fromId { get; set; }
        public User from { get; set; }
        public string message { get; set; }
    }
}