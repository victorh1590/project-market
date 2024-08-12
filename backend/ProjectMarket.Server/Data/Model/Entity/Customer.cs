using System.Text;
using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.Entity;

public class Customer
{
    public int? CustomerId { get; init; }
    public string Name { get; set; }
    public string Email { get; set; }
    public byte[] Password { get; private set; }
    public string PasswordString
    {
        get => Encoding.UTF8.GetString(Password);
        set => Password = Encoding.ASCII.GetBytes(value);
    }
    public DateTime RegistrationDate { get; set; }
    
    public Customer(
        int? CustomerId, 
        string Name, 
        string Email, 
        byte[] Password, 
        DateTime? RegistrationDate)
    {
        this.CustomerId = CustomerId;
        this.Name = Name;
        this.Email = Email;
        this.Password = Password;
        this.RegistrationDate = RegistrationDate ?? DateTime.Now;

        this.Validate();
    }
    
    public Customer(
        int? id, 
        string name, 
        string email, 
        string password, 
        DateTime? registrationDate)
    {
        CustomerId = id;
        Name = name;
        Email = email;
        Password = Encoding.ASCII.GetBytes(password);
        RegistrationDate = registrationDate ?? DateTime.Now;

        this.Validate();
    }
}

public static class CustomerExtension {
    private static CustomerValidator Validator { get; } = new();

    public static void Validate(this Customer customer) => 
        Validator.ValidateAndThrow(customer);
}
