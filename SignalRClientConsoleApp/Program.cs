using Microsoft.AspNetCore.SignalR.Client;
using SignalRClientConsoleApp;

//console app tarafından signalR hubuma nasıl connect olunur.

Console.WriteLine("SignalR Console Client");

var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7223/exampleTypeSafeHub")
    .Build();

connection.StartAsync().ContinueWith(async (Task) =>
{
    Console.WriteLine(Task.IsCompletedSuccessfully ? "Connected" : "Connection failed");
});

connection.On<Product>("ReceiveTypedMessageForAllClient", (product) =>
{
    Console.WriteLine($"Received message: {product.id}-{product.name}-{product.price}");
});

//bu döngüde herhangi bir karaktere bastığımızda hubumuza belirledigimiz mesaji gönderir.
while (true)
{
    var key = Console.ReadLine();
    if (key == "exit") break;
    var newProduct = new Product(200, "pen 200", 250);

    await connection.InvokeAsync("BroadcastTypedMessageToAllClient", newProduct);
}