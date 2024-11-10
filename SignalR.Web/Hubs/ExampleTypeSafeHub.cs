using Microsoft.AspNetCore.SignalR;
using SignalR.Web.Models;

namespace SignalR.Web.Hubs
{
    public class ExampleTypeSafeHub : Hub<IExampleTypeSafeHub>
    {
        //clienta kaç tane connect var onu tutmak için bir değişken tanımlandı.
        private static int ConnectedClientCount = 0;

        //bu metot ise hub tarafından çalıştırılarak tüm client tarafını tetikleyecek.
        public async Task BroadcastMessageToAllClient(string message)
        {
            //tip güvenlikli tanımlamak için interface kullanıldı.
            await Clients.All.ReceiveMessageForAllClient(message);
        }

        public async Task BroadcastTypedMessageToAllClient(Product product)
        {
            await Clients.All.ReceiveTypedMessageForAllClient(product);
        }

        //streaming işlemi için kullanılan metot. içerisine gelen chunkları tek tek clientlara gönderir.
        public async Task BroadcastStreamDataToAllClient(IAsyncEnumerable<string> nameAsChunk)
        {
            await foreach (var name in nameAsChunk)
            {
                await Task.Delay(1000);
                await Clients.All.ReceiveMessageAsStreamForAllClient(name);
            }
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

        //Belirli bir connectionId'ye sahip client'a mesaj göndermek için kullanılan metot.
        public async Task BroadcastMessageToIndividualClient(string connectionId, string message)
        {
            await Clients.Client(connectionId).ReceiveMessageForIndividualClient(message);
        }

        //Grup içerisinden bir client mesaj gönderdiğinde çağrılacak olan metot.
        public async Task BroadcastMessageToGroupClients(string groupName, string message)
        {
            await Clients.Group(groupName).ReceiveMessageForGroupClients(message);
        }

        //Gruba dahil olma işlemi için kullanılan metot.
        public async Task AddGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            //gruba eklendiğinde eklenen kişiye gönderilecek mesaj
            await Clients.Caller.ReceiveMessageForCallerClient($"{groupName} grubuna dahil oldunuz.");
            //gruba dahil olduğunda tüm clientlara gönderilecek mesaj
            await Clients.Others.ReceiveMessageForOthersClient($"Kullanıcı{Context.ConnectionId} {groupName} grubuna dahil oldu.");

            //gruba dahil olan kişinin olduğu grubun tüm clientlarına gönderilecek mesaj
            await Clients.Group(groupName).ReceiveMessageForGroupClients($"Kullanıcı{Context.ConnectionId} {groupName} grubuna dahil oldu.");
        }

        public async Task RemoveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.ReceiveMessageForCallerClient($" {groupName} grubundan çıktınız.");
            await Clients.Others.ReceiveMessageForOthersClient($"Kullanıcı{Context.ConnectionId} {groupName} grubundan çıktı.");
            await Clients.Group(groupName).ReceiveMessageForGroupClients($"Kullanıcı{Context.ConnectionId} {groupName} grubundan çıktı.");
        }
    }
}