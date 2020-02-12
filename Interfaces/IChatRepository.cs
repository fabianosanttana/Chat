using System;
using System.Collections.Generic;
using Chat.Models;

namespace Chat.Interfaces
{
    public interface IChatRepository
    {
         User Add(string ConnectionId, User user);
         void Disconnect(string ConnectionId);
         IList<User> GetUsers();
         User GetUserByKey(Int64 key);
    }
}