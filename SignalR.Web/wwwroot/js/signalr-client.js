//client huba bağlanmak için kullanılır.
const connection = new signalR.HubConnectionBuilder().withUrl("/exampleTypeSafeHub").configureLogging(signalR.LogLevel.Information).build();

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

    const receiveTypedMessageForAllClient = "ReceiveTypedMessageForAllClient";
    const broadcastTypedMessageToAllClient = "BroadcastTypedMessageToAllClient";

    ///grup işlemleri
    const groupA = "GroupA";
    const groupB = "GroupB";
    let currentGroupList = [];
    async function start() {
        try {
            await connection.start().then(() => {
                console.log("Hub ile bağlantı kuruldu!");
                $("#connectionId").html(`Connection Id: ${connection.connectionId}`);
            });
        }
        catch (err) {
            console.error("hubn ile bağlantı kurulamadı", err);
            setTimeout(() => start(), 5000);
        }
    }

    //connection kopar ise tekrar bağlanması için
    connection.onclose(async () => {
        await start();
    })

    start();
    function refleshGroupList() {
        $("#groupList").empty();
        currentGroupList.forEach(x => {
            $("#groupList").append(`<p>${x}</p>`);
        })
    }

    $("#btn-groupA-add").click(function () {
        if (currentGroupList.includes(groupA)) return;

        connection.invoke("AddGroup", groupA).then(() => {
            currentGroupList.push(groupA);
            refleshGroupList();
        })
    })

    $("#btn-groupA-remove").click(function () {
        if (!currentGroupList.includes(groupA)) return;

        connection.invoke("RemoveGroup", groupA).then(() => {
            currentGroupList = currentGroupList.filter(x => x !== groupA);
            refleshGroupList();
        })
    })

    $("#btn-groupB-add").click(function () {
        if (currentGroupList.includes(groupB)) return;

        connection.invoke("AddGroup", groupB).then(() => {
            currentGroupList.push(groupB);
            refleshGroupList();
        })
    })

    $("#btn-groupB-remove").click(function () {
        if (!currentGroupList.includes(groupB)) return;

        connection.invoke("RemoveGroup", groupB).then(() => {
            currentGroupList = currentGroupList.filter(x => x !== groupB);
            refleshGroupList();
        })
    })

    $("#btn-groupA-send-message").click(function () {
        const message = "Group A mesaj";
        connection.invoke("BroadcastMessageToGroupClients", groupA, message).catch(err =>
            console.error("hata", err))
        console.log("Mesaj gönderildi.");
    })

    $("#btn-groupB-send-message").click(function () {
        const message = "Group B mesaj";
        connection.invoke("BroadcastMessageToGroupClients", groupB, message).catch(err =>
            console.error("hata", err))
        console.log("Mesaj gönderildi.");
    })

    connection.on("ReceiveMessageForGroupClients", (message) => {
        console.log("Gelen Mesaj: ", message);
    })

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

    connection.on(receiveTypedMessageForAllClient, (product) => {
        console.log("(Individual) Gelen Mesaj: ", product);
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

    $("#btn-send-typed-message-all-client").click(function () {
        const product = { id: 1, name: "Product 1", price: 100 };
        connection.invoke(broadcastTypedMessageToAllClient, product).catch(err => console.error("hata", err))
        console.log("Mesaj gönderildi.");
    });
})