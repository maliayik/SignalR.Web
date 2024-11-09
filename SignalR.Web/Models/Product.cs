namespace SignalR.Web.Models
{
    //sadece bir data tutucaksa record kullanılabilir.
    public record Product(int id,string name,decimal price);
   
}
