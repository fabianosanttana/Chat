using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Chat
{
   public class ChatHub : Hub
    {
         /// <summary>
        /// Método responsável por encaminhar as mensagens pelo hub
        /// </summary>
        /// <param name="channel">Este parâmetro será a ponte entre duas conexões</param>
        /// <param name="user">Quem está enviando a mensagem</param>
        /// <param name="message">A mensagem enviada</param>
        /// <returns></returns>
        public async Task SendMessage(string channel, string user, string message)
        {
            await Clients.All.SendAsync(channel, user, message);
        }
    }
}