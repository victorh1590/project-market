namespace ProjectMarket.Server.Data.Model.Entity;

public class Costumer {
    public int CostumerId { get; set; }
    required public string Name { get;set; }
    required public string Email { get; set; }
    required public string Password { get; set; }
    public DateTime RegistrationDate { get; set; }
}