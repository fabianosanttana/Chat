using System;

namespace Chat.Models
{
    public class ChatMessage
    {
        public Int64 destination { get; set; }   
        public User sender {get;set;}       
        public string message {get;set;}   
    }
    public class User{
        public string name {get;set;}
        public Int64 key { get; set; } 
        public DateTime dtConnection { get; set; }       
    }
}