using Microsoft.AspNetCore.SignalR;

namespace SignalR.Web.Hubs
{
    public class ExampleTypeSafeHub:Hub<IExampleTypeSafeHub>
    {
        //bu metot ise hub tarafından çalıştırılarak client tarafını tetikleyecek.
        public async Task BroadcastMessageToAllClient(string message)
        {
            //tip güvenlikli tanımlamak için interface kullanıldı.
            await Clients.All.ReceiveMessageForAllClient(message);


            // await Clients.All.SendAsync("ReceiveMessageForAllClient", message);

        }
    }
}
