namespace OrderService.Entities;

public class Order
{
    public int Id { get; set; }

    public int Productİd { get; set; }

    public double Quantity { get; set; }

    public DateTime OrderDate { get; set; }

    public double TotalPrice { get; set; }
}
