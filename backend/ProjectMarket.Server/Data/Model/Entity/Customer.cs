using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.Entity;

public class Customer {
    required public int CustomerId { get; set; }
    required public string Name { get;set; }
    required public string Email { get; set; }
    required public string Password { get; set; }
    public DateTime RegistrationDate { get; set; }

     public Customer(
        int id, 
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

        var validator = new CustomerValidator();
        validator.ValidateAndThrow(this);
    }
}