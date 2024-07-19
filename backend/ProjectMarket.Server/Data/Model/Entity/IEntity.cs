using FluentValidation;
using ProjectMarket.Server.Data.Validators;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Data.Model.Entity;

public interface IEntity {
    public void ValidateId();
}
