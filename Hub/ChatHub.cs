using Chat.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using Chat.Repositories;
using Newtonsoft.Json;
using Chat.Interfaces;

namespace Chat.Hubs
{
   public class ChatHub : Hub
    {
        private readonly IChatRepository _repository;

        public ChatHub(IChatRepository chatRepository) => _repository = chatRepository;

        /// <summary>
        /// Override para inserir cada usuário no nosso repositório, lembrando que esse repositório está em memória
        /// </summary>
        /// <returns> Retorna lista de usuário no chat e usuário que acabou de logar </returns>
        public override Task OnConnectedAsync()
        {
            var user = JsonConvert.DeserializeObject<User>(Context.GetHttpContext().Request.Query["user"]);
            _repository.Add(Context.ConnectionId, user);
            
            //Ao usar o método All eu estou enviando a mensagem para todos os usuários conectados no meu Hub
            Clients.All.SendAsync("chat", _repository.GetUsers(), user);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            _repository.Disconnect(Context.ConnectionId);
            Clients.All.SendAsync("chat", _repository.GetUsers());

            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Método responsável por encaminhar as mensagens pelo hub
        /// </summary>
        /// <param name="ChatMessage">Este parâmetro é nosso objeto representando a mensagem e os usuários envolvidos</param>
        /// <returns></returns>
        public async Task SendMessage(ChatMessage chat)
        {
            await Clients.Client(_repository.GetUserByKey(chat.destination).key.ToString()).SendAsync("Receive", chat.sender, chat.message);
        }
    }
}