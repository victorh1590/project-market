using ProjectMarket.Server.Data.Model.Dto;
using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Model.Factory;

public class CustomerFactory
{
    public Customer CreateCustomer(CustomerDto dto) => new(dto.CustomerId, dto.Name, dto.Email, dto.Password, dto.RegistrationDate);
}