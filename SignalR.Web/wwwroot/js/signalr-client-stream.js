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

    connection.on(receiveMessageAsStreamForAllClient, (name) => {
        $("#streamBox").append(`<p>${name}</p>`);
    });

    //chunklarımızı hubumuza göndermeye yarayan buton
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

    start();
});