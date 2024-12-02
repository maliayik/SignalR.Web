using Microsoft.Extensions.FileProviders;
using SampleProjectWeb.Models;
using System.Data;
using System.Threading.Channels;
using ClosedXML.Excel;
using DataTable = System.Data.DataTable;

namespace SampleProjectWeb.BackgroundServices
{
    public class CreateExcelBackgroundService(
        Channel<(string userId, List<Product> products)> channel,
        IFileProvider fileProvider,
        IServiceProvider serviceProvider) : BackgroundService
    {
        //uygulama ayağa kalktığında 1 kez çalışacak metot
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //kuyruk içerisine mesaj gelene kadar bekler ne zaman gelirse döngünün içerisine girer.
            while (await channel.Reader.WaitToReadAsync(stoppingToken))
            {
                var (userId, products) = await channel.Reader.ReadAsync(stoppingToken);

                var wwwrootfolder = fileProvider.GetDirectoryContents("wwwroot");

                var files = wwwrootfolder.Single(x => x.Name == "files");

                var newExcelFileName = $"product-list-{Guid.NewGuid()}.xlsx";

                var newExcelFilePath = Path.Combine(files.PhysicalPath, newExcelFileName);

                var wb = new XLWorkbook();

                //veritabanımıza karşılık gelen bir dataset oluşturuyoruz.
                var ds = new DataSet();

                ds.Tables.Add(GetTable("Product List", products));

                wb.Worksheets.Add(ds);

                await using var excelFileStream = new FileStream(newExcelFilePath, FileMode.Create);

                wb.SaveAs(excelFileStream);

            }
        }

        private DataTable GetTable(string tableName, List<Product> products)
        {
            //in memomry datatable oluşturuyoruz.
            var table = new DataTable { TableName = tableName };

            //tablonun sutunlarını olustur ve tipini belirle
            foreach (var item in typeof(Product).GetProperties()) table.Columns.Add(item.Name, item.PropertyType);

            //tabloya verileri ekle
            products.ForEach(x => { table.Rows.Add(x.Id, x.Name, x.Price, x.Description,x.UserId);});

            return table;
            
        }
    }
}
