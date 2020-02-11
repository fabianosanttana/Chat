namespace Chat.Commums
{
    using Chat.Context;
    using Chat.Interfaces;
    using Chat.Repositories;
    using Microsoft.Extensions.DependencyInjection;
    public static class DependencyInjections
    {
        /// <summary>
        /// Método responsável por realizar o bind de dependências
        /// </summary>
        /// <param name="services">Coleção de serviços do startup</param>
        public static void AddDependencyInjections(this IServiceCollection services)
        {
            /* Repositories */
            services.AddScoped<IChatRepository, ChatRepository>();

            /* Contexts */
            services.AddDbContext<ChatContext>();
        }
    }
}