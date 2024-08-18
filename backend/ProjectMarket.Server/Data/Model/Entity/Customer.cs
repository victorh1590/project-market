using System.Text;
using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.Entity;

public record CustomerRecord(int CustomerId, string Name, string Email, byte[] Password, DateTime RegistrationDate);
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
        int? id, 
        string name, 
        string email, 
        string password, 
        DateTime? registrationDate)
    {
        CustomerId = id;
        Name = name;
        Email = email;
        Password = Encoding.UTF8.GetBytes(password);
        RegistrationDate = registrationDate ?? DateTime.Now;
    
        this.Validate();
    }

    public Customer(CustomerRecord record)
    {
        CustomerId = record.CustomerId;
        Name = record.Name;
        Email = record.Email;
        Password = record.Password;
        RegistrationDate = record.RegistrationDate;
    }
    
    public override bool Equals(object? other)
    {
        if (other == null || other is not Customer) return false;
        var otherCustomer = (Customer)other;
        
        return CustomerId == otherCustomer.CustomerId &&
               Name == otherCustomer.Name &&
               Email == otherCustomer.Email &&
               Password.SequenceEqual(otherCustomer.Password) &&
               RegistrationDate == otherCustomer.RegistrationDate;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CustomerId, Name, Email, Password, RegistrationDate);
    }

    public bool Equals(Customer? other)
    {
        return other != null &&
               CustomerId == other.CustomerId &&
               Name == other.Name &&
               Email == other.Email &&
               Password.SequenceEqual(other.Password) &&
               RegistrationDate == other.RegistrationDate;
    }
}

public static class CustomerExtension {
    private static CustomerValidator Validator { get; } = new();

    public static void Validate(this Customer customer) => 
        Validator.ValidateAndThrow(customer);
}
