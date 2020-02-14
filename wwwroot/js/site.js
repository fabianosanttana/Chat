$(document).ready(function() {
  window.chat = createChatController();
  window.chat.loadUser();
});
const publicId = 12345678910;
function createChatController() {
  var user = {
    name: null,
    dtConnection: null,
    key: null
  };

  return {
    state: user,
    connection: null,
    loadUser: function() {
      this.state.name = prompt(
        "Digite seu apelido para entrar no chat",
        "Usuário"
      );
      this.state.dtConnection = new Date();
      this.state.key = new Date().valueOf();
      this.connectUserToChat();
    },
    connectUserToChat: function() {
      //Aqui iniciamos a conexão e deixamos ela aberta
      startConnection(this);
    },
    sendMessage: function(to) {
      var chatMessage = {
        from: this.state,
        message: to.message,
        toId: to.destination
      };

      //Esse trecho é responsável por encaminhar a mensagem para o usuário selecionado
      this.connection
        .invoke("SendMessage", chatMessage)
        .catch(err => console.log((x = err)));

      //Método responsável por inserir a mensagem no chat
      insertMessage(
        chatMessage.toId,
        "me",
        chatMessage.message,
        chatMessage.from.name
      );
      to.field.val("").focus();
    },
    //Método responsável por receber as mensagens
    onReceiveMessage: function() {
      this.connection.on("Receive", (sender, message) => {
        openChat(null, sender, message, false);
      });

      this.connection.on("Public", (sender, message) => {
        openChat(null, sender, message, true);
      });
    }
  };
}

//Método responsável por realizar a conexão do usuário no nosso Hub
async function startConnection(chat) {
  try {
    chat.connection = new signalR.HubConnectionBuilder()
      .withUrl("/chat?user=" + JSON.stringify(window.chat.state))
      .build();
    await chat.connection.start();

    //Carrega usuários no chat
    loadChat(chat.connection);

    //Caso a conexão caia por algum motivo, esse trecho fará o trabalho para reconectar o cliente
    chat.connection.onclose(async () => {
      await startConnection(chat);
    });

    //Realiza o bind da nossa função para receber mensagem
    chat.onReceiveMessage();
  } catch (err) {
    setTimeout(() => startConnection(chat.connection), 5000);
  }
}

//Função para carregar usuários no chat
async function loadChat(connection) {
  connection.on("chat", (users, user) => {
    const listUsers = data => {
      return users
        .map(u => {
          if (u.key != window.chat.state.key)
            return `
              <section class="user box_shadow_0" onclick="openChat(this)" data-id="${
                u.key
              }" data-name="${u.name}">
              <span class="user_icon">${u.name.charAt(0)}</span>
              <p class="user_name"> ${u.name} </p>
              <span class="user_date"> ${new Date(
                u.dtConnection
              ).toLocaleDateString()}</span>
              </section>
              `;
        })
        .join("");
    };
    verifyOpenChats(users);
    $(".main")
      .empty()
      .append(listUsers);
  });
}
function verifyOpenChats(users) {
  document.querySelectorAll("section.chat").forEach(e => {
    var value = $(e).data("chat");
    if (!users.some(obj => obj.key == value) && $(e).data('chat') != publicId) $(e).remove();
  });
}

//Método responsável por iniciar um novo chat
function openChat(e, sender, message, public) {
  var user = {
    id: e ? $(e).data("id") : sender.key,
    name: e ? $(e).data("name") : sender.name
  };
  if (!checkIfElementExist(public ? publicId : user.id, "chat")) {
    const chat = `
        <section class="chat" data-chat="${public ? publicId : user.id}">
        <header>
            ${public ? "Publico" : user.name}
        </header>
        <main>
        </main>
        <footer>
            <input type="text" placeholder="Digite aqui sua mensagem" data-chat="${
              public ? publicId : user.id
            }">
            <a onclick="sendMessage(this)" data-chat="${
              public ? publicId : user.id
            }">Enviar</a>
        </footer>
        </section>
        `;

    $(".chats_wrapper").append(chat);
  }
  if (public && sender.key == window.chat.state.key) return;
  
  if (sender && message)
    insertMessage(
      public ? publicId : sender.key,
      "their",
      message,
      sender.name
    );
}

//Método responsável por inserir a mensagem no chat
function insertMessage(target, who, message, name) {
  const chatMessage = `
    <div class="message ${who}"><span class="senderNameMessage">${name}</span>${message} <span>${new Date().toLocaleTimeString()}</span></div>
    `;
  $(`section[data-chat="${target}"]`)
    .find("main")
    .append(chatMessage);
}

//Método responsável por capturar a mensagem e enviar
function sendMessage(e) {
  var input = {
    destination: $(e).data("chat"),
    field: $(`input[data-chat="${$(e).data("chat")}"]`),
    message: $(`input[data-chat="${$(e).data("chat")}"]`).val()
  };

  window.chat.sendMessage(input);
}

//Função genérica para verificar se o elemento já existe na DOM
function checkIfElementExist(id, data) {
  return (
    $("section[data-" + data + '="' + id + '"]') &&
    $("section[data-" + data + '="' + id + '"]').length > 0
  );
}
