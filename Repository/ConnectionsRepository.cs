using System.Collections.Generic;
using System.Linq;
using Chat.Models;

namespace Chat.Repository
{
    public class ConnectionsRepository
    {
        private readonly Dictionary<string, User> connections =
            new Dictionary<string, User>();


        public void Add(string uniqueID, User user)
        {
            if (!connections.ContainsKey(uniqueID))
                connections.Add(uniqueID, user);
        }

        public string GetUserId(long id)
        {
            return (from con in connections
            where con.Value.key == id
            select con.Key).FirstOrDefault();
        }

        public List<User> GetAllUser(){
            return (from con in connections
            select con.Value
            ).ToList();
        }

        // public void Remove(T key, string connectionId)
        // {
        //     lock (_connections)
        //     {
        //         HashSet<string> connections;
        //         if (!_connections.TryGetValue(key, out connections))
        //         {
        //             return;
        //         }

        //         lock (connections)
        //         {
        //             connections.Remove(connectionId);

        //             if (connections.Count == 0)
        //             {
        //                 _connections.Remove(key);
        //             }
        //         }
        //     }
        // }
    }
}