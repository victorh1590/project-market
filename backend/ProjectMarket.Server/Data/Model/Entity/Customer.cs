using FluentValidation;
using FluentValidation.Validators;
using ProjectMarket.Server.Data.Model.DTO;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.Entity;

public class Customer {
    public int? CustomerId { get; set; }
    public string Name { get;set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime RegistrationDate { get; set; }

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
        Password = password;
        RegistrationDate = registrationDate ?? DateTime.Now;

        this.Validate();
    }
}

public static class CustomerExtension {
    public static CustomerValidator Validator { get; private set; } = new();

    public static void Validate(this Customer paymentOffer) => 
        Validator.ValidateAndThrow(paymentOffer);
}
