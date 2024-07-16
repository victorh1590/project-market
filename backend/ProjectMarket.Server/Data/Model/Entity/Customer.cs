namespace ProjectMarket.Server.Data.Model.Entity;

public class Customer {
    required public int CustomerId { get; set; }
    required public string Name { get;set; }
    required public string Email { get; set; }
    required public string Password { get; set; }
    public DateTime RegistrationDate { get; set; }
}