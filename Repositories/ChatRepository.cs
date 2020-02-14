using System;
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
    
        public User Add(string connectionHost, User user)
        {
            var logged = _context.Users.Where(obj => obj.connectionHost.Equals(connectionHost)).FirstOrDefault();
            if (logged is null)
            {
                user.connectionHost = connectionHost;
                _context.Users.Add(user);
                _context.SaveChanges();
                return user;
            }
            return logged;
        }

        public void Disconnect(string connectionId)
        {
            _context.Users.Remove(_context.Users.FirstOrDefault(obj => obj.connectionHost.Equals(connectionId)));
            _context.SaveChanges();
        }

        public IList<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserByKey(Int64 key)
        {
            return _context.Users.FirstOrDefault(obj => obj.key.Equals(key));
        }
    }
}