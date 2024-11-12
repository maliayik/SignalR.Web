$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder().withUrl("/exampleTypeSafeHub").configureLogging(signalR.LogLevel.Information).build();
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

    const broadcastStreamDataToAllClient = "BroadcastStreamDataToAllClient";
    const receiveMessageAsStreamForAllClient = "ReceiveMessageAsStreamForAllClient";

    const broadcastStreamProductToAllClient = "BroadcastStreamProductToAllClient";
    const receiveProductAsStreamForAllClient = "ReceiveProductAsStreamForAllClient";

    const broadcastFromHubToClient = "BroadcastFromHubToClient";

    connection.on(receiveMessageAsStreamForAllClient, (name) => {
        $("#streamBox").append(`<p>${name}</p>`);
    });

    connection.on(receiveProductAsStreamForAllClient, (product) => {
        $("#streamBox").append(`<p>${product.id}-${product.name}-${product.price}</p>`);
    });

    //isimleri chunklar ile parça parça clienttan  hubumuza göndermeye yarayan buton
    $("#btn_FromClient_ToHub").click(function () {
        const names = $("#txt_stream").val();

        const nameAsChunk = names.split(";");

        const subject = new signalR.Subject();

        connection.send(broadcastStreamDataToAllClient, subject).catch(err => console.error(err.toString()));

        nameAsChunk.forEach(name => {
            subject.next(name);
        });

        subject.complete();
    });

    //product listemizi chunklar ile parça parça clienttan  hubumuza göndermeye yarayan buton
    $("#btn_FromClient_ToHub2").click(function () {
        const productList = [{ id: 1, name: "Product1", price: 100 }, { id: 2, name: "Product2", price: 200 }, { id: 3, name: "Product3", price: 300 }];

        const subject = new signalR.Subject();

        connection.send(broadcastStreamProductToAllClient, subject).catch(err => console.error(err.toString()));

        productList.forEach(product => {
            subject.next(product);
        });

        subject.complete();
    });

    $("#btn_FromHubToClient").click(function () {
        connection.stream(broadcastFromHubToClient, 5).subscribe({

            next: (message) => $("#streamBox").append(`<p>${message}</p>`)
        });    
    });

    start();
});