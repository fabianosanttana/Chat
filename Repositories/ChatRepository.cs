using System.Collections.Generic;
using System.Linq;
using Chat.Context;
using Chat.Interfaces;
using Chat.Models;

namespace Chat.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatContext _context;
        public ChatRepository(ChatContext context) => _context = context;
    
        public void Add(string connectionHost, User user)
        {
            var logged = _context.Users.Where(obj => obj.connectionHost.Equals(connectionHost)).FirstOrDefault();
            if (logged is null)
            {
                user.connectionHost = connectionHost;
                _context.Users.Add(user);
            }
        }

        public void Disconnect(string connectionId)
        {
            _context.Users.Remove(_context.Users.FirstOrDefault(obj => obj.connectionHost.Equals(connectionId)));
        }

        public IList<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserByKey(long key)
        {
            return _context.Users.FirstOrDefault(obj => obj.key == obj.key);
        }
    }
}