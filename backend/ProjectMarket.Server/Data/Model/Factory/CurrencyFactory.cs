using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Infra.Repository;

namespace ProjectMarket.Server.Data.Model.Factory;

public class CurrencyFactory(IUnitOfWork uow)
{
    private readonly CurrencyRepository _currencyRepository = new(uow);

    public CurrencyVo? CreateCurrencyVo(string currencyName) 
        => _currencyRepository.GetByCurrencyName(currencyName);
}
