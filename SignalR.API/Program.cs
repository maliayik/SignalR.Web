using SignalR.API.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

//API mýzý tarayýcý kullanacaðý için CORS ayarýmýzý yapýyoruz
builder.Services.AddCors(action =>
{
    action.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7223").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors();

app.MapHub<MyHub>("/myhub");

app.UseAuthorization();

app.MapControllers();

app.Run();
