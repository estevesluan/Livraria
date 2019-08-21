using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Livraria.WebApp.Hubs
{
    public class LivroHub : Hub
    {
        public async Task AtualizarListaDeLivros()
        {
            await Clients.All.SendAsync("AtualizarLista");
        }
    }
}
