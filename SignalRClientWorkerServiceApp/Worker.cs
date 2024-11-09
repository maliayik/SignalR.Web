using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRClientWorkerServiceApp
{
    public class Worker(ILogger<Worker> logger, IConfiguration configuration) : BackgroundService
    {
        //normal constructor tanýmlamak yerine primary constructor tanýmlama #C# 9.0 ile birlikte class parametrelerini constructor içerisinde tanýmlayabiliyoruz.

        //private readonly ILogger<Worker> _logger;
        //private readonly IConfiguration configuration;

        //public Worker(ILogger<Worker> logger, IConfiguration configuration)
        //{
        //    _logger = logger;
        //    this.configuration = configuration;
        //}

        private HubConnection? connection;

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            connection = new HubConnectionBuilder()
            .WithUrl(configuration.GetSection("SignalR")["Hub"]!)
            .Build();

            connection?.StartAsync().ContinueWith(async (Task) =>
            {
                logger.LogInformation(Task.IsCompletedSuccessfully ? "Connected" : "Connection failed");
            });

            return base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await connection!.StopAsync(cancellationToken);
            await connection!.DisposeAsync();
            base.StopAsync(cancellationToken);
        }

        //uygulama ayaða kalktýðýnda yalnýzca 1 kere çalýþacak olan metot
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            connection!.On<Product>("ReceiveTypedMessageForAllClient", (product) =>
            {
                logger.LogInformation($"Received message: {product.id}-{product.name}-{product.price}");
            });
            return Task.CompletedTask;
        }
    }
}