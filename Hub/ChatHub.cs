using Chat.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Chat.Interfaces;
using System.Text.Json;
using System;

namespace Chat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatRepository _repository;
        private readonly Int64 publicId = 12345678910;

        public ChatHub(IChatRepository chatRepository) => _repository = chatRepository;

        /// <summary>
        /// Override para inserir cada usuário no nosso repositório, lembrando que esse repositório está em memória
        /// </summary>
        /// <returns> Retorna lista de usuário no chat e usuário que acabou de logar </returns>
        public override Task OnConnectedAsync()
        {
            var userQuery = JsonSerializer.Deserialize<User>(Context.GetHttpContext().Request.Query["user"]);
            var user = _repository.Add(Context.ConnectionId, userQuery);
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
            if (chat.toId.Equals(publicId))
            {
                await Clients.All.SendAsync("Public", chat.from, chat.message);
                return;
            }
            var connection = _repository.GetUserByKey(chat.toId).connectionHost;
            await Clients.Client(connection).SendAsync("Receive", chat.from, chat.message);
        }
    }
}