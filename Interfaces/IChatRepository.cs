using System.Collections.Generic;
using Chat.Models;

namespace Chat.Interfaces
{
    public interface IChatRepository
    {
         void Add(string ConnectionId, User user);
         void Disconnect(string ConnectionId);
         IList<User> GetUsers();
         User GetUserByKey(long key);
    }
}