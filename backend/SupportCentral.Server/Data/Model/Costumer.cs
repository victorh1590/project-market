namespace SupportCentral.Server.Data.Model;

public class Costumer {
    public int Id { get; set; }
    public string Name { get;set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime RegistrationDate { get; set; }
    public List<ProjectAdvertisement>? Advertisements { get; set; }
}