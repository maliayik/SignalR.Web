
//bu metot tüm sayfa yüklendikten sonra çalışacak olan metottur.
$(document).ready(function () {
    const broadcastMessageToAllClientHubMethodCall = "BroadcastMessageToAllClient";
    const receiveMessageForAllClientMethodCall = "ReceiveMessageForAllClient";
    const receiveConnectedClientCountAllClient = "ReceiveConnectedClientCountAllClient";

    //client huba bağlanmak için kullanılır.
    const connection = new signalR.HubConnectionBuilder().withUrl("/exampleTypeSafeHub").configureLogging(signalR.LogLevel.Information).build();

    function start() {
        connection.start().then(() =>
            console.log("Hub ile bağlantı kuruldu!"));
    }
    try {
        start();
    }
    catch {
        setTimeout(() => start(), 5000);
    }

    //hub tarafından client'a mesaj gönderildiğinde çalışacak olan metotda subscribe olunur.
    connection.on(receiveMessageForAllClientMethodCall, (message) => {
        console.log("Gelen Mesaj: ", message);
    })


    var span_client_count = $("#span-connected-client-count");

    connection.on(receiveConnectedClientCountAllClient, (count) => {
        span_client_count.text(count);
        console.log("connected client count:", count);
    })


    $("#btn-send-message-all-client").click(function () {

        const message = "Hello World!";

        connection.invoke(broadcastMessageToAllClientHubMethodCall, message).catch(err => console.error("hata", err))
    })
})