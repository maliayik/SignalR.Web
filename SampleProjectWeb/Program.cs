using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SampleProjectWeb.BackgroundServices;
using SampleProjectWeb.Models;
using SampleProjectWeb.Services;
using System.Threading.Channels;
using SampleProjectWeb.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

//tuple tanýmlama 1. yol channela göndericeðimiz mesaj içeriðini veriyoruz (Unbounded olduðu için  channel sýnýrsýz mesaj alabilir bounded olsaydý belirli bir sayýda mesaj alýrdý.)
builder.Services.AddSingleton(Channel.CreateUnbounded<(string userId,List<Product> products)>());
//2. yol
//builder.Services.AddSingleton(Channel.CreateUnbounded<Tuple<string,List<Product>>>());
builder.Services.AddScoped<FileService>();
builder.Services.AddHttpContextAccessor();

//wwwroot klasörünü kullanabilmek için
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

builder.Services.AddHostedService<CreateExcelBackgroundService>();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapHub<AppHub>("/hub");

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
