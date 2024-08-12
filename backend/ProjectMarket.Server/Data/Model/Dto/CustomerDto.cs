using System.Text;

namespace ProjectMarket.Server.Data.Model.Dto;

public class CustomerDto
{
    public int? CustomerId { get; init; }
    public string Name { get; set; }
    public string Email { get; set; }
    public byte[] Password { get; private set; }
    public string PasswordString
    {
        get => Encoding.UTF8.GetString(Password);
        set => Password = Encoding.UTF8.GetBytes(value);
    }
    public DateTime RegistrationDate { get; set; }
    
    public CustomerDto(
        int? id, 
        string name, 
        string email, 
        byte[] password, 
        DateTime? registrationDate)
    {
        CustomerId = id;
        Name = name;
        Email = email;
        Password = password;
        RegistrationDate = registrationDate ?? DateTime.Now;
    }
    
    public CustomerDto(
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
    }
}