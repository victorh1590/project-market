using FluentValidation;
using FluentValidation.Validators;
using ProjectMarket.Server.Data.Model.DTO;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.Entity;

public class Customer : IEntity {
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

        var validator = new CustomerValidator();
        validator.ValidateAndThrow(this);
    }

    public static Customer CreateCustomer(CustomerDTO dto)
    {
        return new Customer(
            dto.CustomerId, 
            dto.Name, 
            dto.Email, 
            dto.Password, 
            dto.RegistrationDate
        );
    }

    public void ValidateId() {
        var validator = new NotNullValidator<int?>();
        validator.ValidateAndThrow(CustomerId);
    }
}