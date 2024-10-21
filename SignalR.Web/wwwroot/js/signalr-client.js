
//bu metot tüm sayfa yüklendikten sonra çalışacak olan metottur.
$(document).ready(function () {
    const broadcastMessageToAllClientHubMethodCall = "BroadcastMessageToAllClient";
    const receiveMessageForAllClientMethodCall = "ReceiveMessageForAllClient";

    const broadcastMessageToCallerClient = "BroadcastMessageToCallerClient";
    const receiveMessageForCallerClient = "ReceiveMessageForCallerClient";

    const broadcastMessageToOthersClient = "BroadcastMessageToOthersClient";
    const receiveMessageForOthersClient = "ReceiveMessageForOthersClient";


    const receiveConnectedClientCountAllClient = "ReceiveConnectedClientCountAllClient";

    const broadcastMessageToIndividualClient = "BroadcastMessageToIndividualClient";
    const receiveMessageForIndividualClient = "ReceiveMessageForIndividualClient";



    //client huba bağlanmak için kullanılır.
    const connection = new signalR.HubConnectionBuilder().withUrl("/exampleTypeSafeHub").configureLogging(signalR.LogLevel.Information).build();

    function start() {
        connection.start().then(() => {
            console.log("Hub ile bağlantı kuruldu!");
            $("#connectionId").html(`Connection Id: ${connection.connectionId}`);
        });
    }
    try {
        start();
    }
    catch {
        setTimeout(() => start(), 5000);
    }






    //subcribers
    //hub tarafından client'a mesaj gönderildiğinde çalışacak olan metotda subscribe olunur.
    connection.on(receiveMessageForAllClientMethodCall, (message) => {
        console.log("Gelen Mesaj: ", message);
    })

    connection.on(receiveMessageForCallerClient, (message) => {
        console.log("(Caller) Gelen Mesaj: ", message);
    })

    connection.on(receiveMessageForOthersClient, (message) => {
        console.log("(Others) Gelen Mesaj: ", message);
    })

    connection.on(receiveMessageForIndividualClient, (message) => {
        console.log("(Individual) Gelen Mesaj: ", message);
    })

    var span_client_count = $("#span-connected-client-count");

    connection.on(receiveConnectedClientCountAllClient, (count) => {
        span_client_count.text(count);
        console.log("connected client count:", count);
    })


    $("#btn-send-message-all-client").click(function () {

        const message = "Hello World!";
        connection.invoke(broadcastMessageToAllClientHubMethodCall, message).catch(err => console.error("hata", err))
        console.log("Mesaj gönderildi.");
    })


    $("#btn-send-message-caller-client").click(function () {

        const message = "Hello World!";
        connection.invoke(broadcastMessageToCallerClient, message).catch(err => console.error("hata", err))
        console.log("Mesaj gönderildi.");
    })

    $("#btn-send-message-others-client").click(function () {

        const message = "Hello World!";
        connection.invoke(broadcastMessageToOthersClient, message).catch(err => console.error("hata", err))
        console.log("Mesaj gönderildi.");
    })

    $("#btn-send-message-individual-client").click(function () {

        const message = "Hello World!";
        const connectionId = $("#text-connectionId").val();
        connection.invoke(broadcastMessageToIndividualClient, connectionId, message).catch(err => console.error("hata", err))
        console.log("Mesaj gönderildi.");
    })
})