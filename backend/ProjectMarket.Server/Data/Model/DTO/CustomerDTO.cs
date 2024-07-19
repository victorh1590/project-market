namespace ProjectMarket.Server.Data.Model.DTO;

public class CustomerDTO(
   int? id,
   string name,
   string email,
   string password,
   DateTime? registrationDate)
{
    public int? CustomerId { get; set; } = id;
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
    public DateTime RegistrationDate { get; set; } = registrationDate ?? DateTime.Now;
}