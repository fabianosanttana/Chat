using System;
using System.ComponentModel.DataAnnotations;

namespace Chat.Models
{
    public class User
    {
        public string name {get;set;}
        [Key]
        public Int64 key { get; set; } 
        public DateTime dtConnection { get; set; }
        public string connectionHost {get; set;}       
    }
}