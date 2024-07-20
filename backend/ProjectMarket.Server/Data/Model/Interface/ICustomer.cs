namespace ProjectMarket.Server.Data.Model.Interface;

public interface ICustomer
{
    int? CustomerId { get; }
    string Name { get; }
    string Email { get; }
    string Password { get; }
    DateTime RegistrationDate { get; }
}