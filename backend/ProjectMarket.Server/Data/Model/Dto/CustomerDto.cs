using ProjectMarket.Server.Data.Model.Interface;

namespace ProjectMarket.Server.Data.Model.Dto.EntityDto;

public class CustomerDto(
   int? id,
   string name,
   string email,
   string password,
   DateTime? registrationDate) : ICustomer
{
    public int? CustomerId { get; } = id;
    public string Name { get; } = name;
    public string Email { get; } = email;
    public string Password { get; } = password;
    public DateTime RegistrationDate { get; } = registrationDate ?? DateTime.Now;
}