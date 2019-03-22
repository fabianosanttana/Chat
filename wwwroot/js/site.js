
async function startConnection(connection) {
    try {
        connection = new signalR.HubConnectionBuilder().withUrl("/chat?user=" + JSON.stringify(window.chat.state)).build();
        await connection.start();
        loadChat(connection);

    } catch (err) {
        setTimeout(() => startConnection(connection), 5000);
    }
};

function openChat(e){
    var id =  $(e).data('id');
    
}

function checkIfExist(id){
    return $('section[data-id="'+id+'"]') && $('section[data-id="'+id+'"]').length > 0;
}

async function loadChat(connection){
    connection.on('chat', (users) => {
        $.each( users, function( key, value ) {
            if(!checkIfExist(value.key)){
            var user = '<section class="user box_shadow_0" onclick="openChat(this)" data-id="'+value.key+'">'+
            '        <span class="user_icon">'+value.name.charAt(0)+'</span>'+
            '        <p class="user_name">'+value.name+'</p>'+
            '        <span class="user_date">'+new Date(value.dtConnection).toLocaleDateString()+'</span>'+
            '    </section>';
            $('.main').append(user);
            }
          });
    });
}

document.getElementById("sendMessage").addEventListener("click", event => {
    window.chat.sendMessage();
    event.preventDefault();
});

$(document).ready(function(){
    window.chat = createChatController();
    window.chat.loadUser();
});

function createChatController(){
    var user = {
        key: null,
        name: null,
        dtConnection: null
    }

    return {
        state : user,
        connection: null,
        loadUser: function(){
            this.state.name =  prompt('Digite seu nome para entrar no chat', '');
            this.state.key = new Date().valueOf();
            this.state.dtConnection = new Date();
            this.connectUserToChat();
        },
        connectUserToChat: function(){

            //Aqui iniciamos a conexão e deixamos ela aberta 
            startConnection(this.connection);

            //Caso a conexão caia por algum motivo, esse trecho fará o trabalho para reconectar o cliente
            this.connection.onclose(async () => {
                await startConnection(connection);
            });

            // Inicializa o recebimento de mensagens para nosso user 
            this.onReceiveMessage();

        },
        sendMessage: function(){

            var chatMessage = {
                sender: this.state,
                message: document.getElementById("userMessage").value,
                destination:this.state.key
            };

            //Esse trecho é responsável por encaminhar a mensagem para o usuário selecionado
            //O primeiro parâmetro é o nome do método no nosso hub
            //A partir do segundo parâmetro são os parâmetros do método (string channel, User sender, string message)
            this.connection.invoke("SendMessage", (chatMessage))
                             .catch(err => console.log(x = err));

        },
        onReceiveMessage: function() {
            this.connection.on("Receive", (sender, message) => {
                console.log(y = suser);
            });
        }
    };
}