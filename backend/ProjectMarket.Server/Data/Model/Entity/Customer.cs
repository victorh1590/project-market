using System.Text;
using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.Entity;

public class Customer : IEquatable<Customer>
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
        int? customerId, 
        string name, 
        string email, 
        byte[] password, 
        DateTime? registrationDate)
    {
        CustomerId = customerId;
        Name = name;
        Email = email;
        Password = password;
        RegistrationDate = registrationDate ?? DateTime.Now;

        this.Validate();
    }
    
    public Customer(
        int? customerId, 
        string name, 
        string email, 
        string password, 
        DateTime? registrationDate)
    {
        CustomerId = customerId;
        Name = name;
        Email = email;
        Password = Encoding.ASCII.GetBytes(password);
        RegistrationDate = registrationDate ?? DateTime.Now;
    
        this.Validate();
    }
    
    public override bool Equals(object? obj) => obj is Customer other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(CustomerId, Name, Email, Password, RegistrationDate);
    public bool Equals(Customer? other) 
        => other != null &&
           CustomerId == other.CustomerId &&
           Name == other.Name &&
           Email == other.Email &&
           Password.SequenceEqual(other.Password) &&
           RegistrationDate == other.RegistrationDate;
}

public static class CustomerExtension {
    private static CustomerValidator Validator { get; } = new();

    public static void Validate(this Customer customer) => 
        Validator.ValidateAndThrow(customer);
}
