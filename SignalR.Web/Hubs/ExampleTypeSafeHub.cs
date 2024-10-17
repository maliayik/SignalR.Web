using Microsoft.AspNetCore.SignalR;

namespace SignalR.Web.Hubs
{
    public class ExampleTypeSafeHub:Hub<IExampleTypeSafeHub>
    {
        //clienta kaç tane connect var onu tutmak için bir değişken tanımlandı.
        private static int ConnectedClientCount = 0;

        //bu metot ise hub tarafından çalıştırılarak tüm client tarafını tetikleyecek.
        public async Task BroadcastMessageToAllClient(string message)
        {
            //tip güvenlikli tanımlamak için interface kullanıldı.
            await Clients.All.ReceiveMessageForAllClient(message);
                    
        }

        //hub'a kaç client bağlandğı bilgisini tutmak için kullanılan metot.
        public override async Task OnConnectedAsync()
        {
            ConnectedClientCount++;

            await Clients.All.ReceiveConnectedClientCountAllClient(ConnectedClientCount);
            await base.OnConnectedAsync(); 
        }

        //hub'dan ayrılan client sayısını tutmak için kullanılan metot.
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectedClientCount--;

           await Clients.All.ReceiveConnectedClientCountAllClient(ConnectedClientCount);
           await base.OnDisconnectedAsync(exception);
        }

        //Hub'u sadece çağıran client'a mesaj göndermek için kullanılan metot.
        public async Task BroadcastMessageToCallerClient(string message)
        {            
            await Clients.Caller.ReceiveMessageForCallerClient(message);
        }

        //Hubu çağıran client hariç diğer clientlara mesaj göndermek için kullanılan metot.
        public async Task BroadcastMessageToOthersClient(string message)
        {
            await Clients.Others.ReceiveMessageForOthersClient(message);
        }
    }
}
 