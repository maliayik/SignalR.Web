using Microsoft.AspNetCore.SignalR;

namespace SignalR.Web.Hubs
{
    public class ExampleTypeSafeHub:Hub<IExampleTypeSafeHub>
    {
        //clienta kaç tane connect var onu tutmak için bir değişken tanımlandı.
        private static int ConnectedClientCount = 0;

        //bu metot ise hub tarafından çalıştırılarak client tarafını tetikleyecek.
        public async Task BroadcastMessageToAllClient(string message)
        {
            //tip güvenlikli tanımlamak için interface kullanıldı.
            await Clients.All.ReceiveMessageForAllClient(message);
                    
        }

        public override async Task OnConnectedAsync()
        {
            ConnectedClientCount++;

            await Clients.All.ReceiveConnectedClientCountAllClient(ConnectedClientCount);
            await base.OnConnectedAsync(); 
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectedClientCount--;

           await Clients.All.ReceiveConnectedClientCountAllClient(ConnectedClientCount);
           await base.OnDisconnectedAsync(exception);
        }
    }
}
 