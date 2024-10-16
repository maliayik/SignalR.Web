using Microsoft.AspNetCore.SignalR;

namespace SignalR.Web.Hubs
{
    public class ExampleHub:Hub
    {
        //Bu metot client tarafından çağrılacak,bu metot da client tarafını tetikleyecek.
        public async Task BroadcastMessageToAllClient(string message)
        {
            await Clients.All.SendAsync("ReceiveMessageForAllClient", message);

        }
    }
}
