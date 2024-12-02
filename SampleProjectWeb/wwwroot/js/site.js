var toastTimeout; //global bir değişken tanımlıyoruz timeout süresi belirtmek için toast mesajlarında kullanacağız.


$(document).ready(function () {

    const connection = new window.signalR.HubConnectionBuilder().withUrl("/hub").build();

    connection.start().then(function () {
        console.log("Bağlantı sağlandı.")
    })

    connection.on("AlertCompleteFile", (downloadPath) => {
        clearTimeout(toastTimeout);

        $(".toast-body").html(`<p>Excel oluşturma işlemi tamamlanmıştır. Aşağıdaki link ile excel dosyasını indirebilirsiniz<p>
        <a href="${downloadPath}" class="btn btn-primary">İndir</a>`);

        $("#liveToast").show();
    });
});