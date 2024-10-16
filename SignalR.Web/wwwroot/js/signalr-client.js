
//bu metot tüm sayfa yüklendikten sonra çalışacak olan metottur.
$(document).ready(function () {

    //client huba bağlanmak için kullanılır.
    const connection = new signalR.HubConnectionBuilder().withUrl("/examplehub").configureLogging(signalR.LogLevel.Information).build();

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
})