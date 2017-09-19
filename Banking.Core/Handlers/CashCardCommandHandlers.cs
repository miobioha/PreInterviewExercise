using Banking.Core.Aggregates;
using Banking.Core.Commands;
using Banking.Core.Repositories;
using Banking.Core.Services;

namespace Banking.Core.Handlers
{
    public class CashCardCommandHandlers
    {
        private readonly ICashCardRepositoryFactory _factory;
        private readonly IHashingService _hasingService;

        public CashCardCommandHandlers(ICashCardRepositoryFactory factory, IHashingService hasingService)
        {
            _factory = factory;
            _hasingService = hasingService;
        }

        public void Handle(CreateCashCard command)
        {
            using (var cashCardRepository = _factory.Create())
            {
                var pinHash = _hasingService.Hash(command.Pin);
                var cashCard = new CashCard(command.Id, command.AccountId, pinHash);

                cashCardRepository.Save(cashCard);
            }
        }

        public void Handle(ChangePin command)
        {
            using (var cashCardRepository = _factory.Create())
            {
                var cashCard = cashCardRepository.GetById(command.Id);
                cashCard.ChangePin(command.NewPinHash);

                cashCardRepository.Save(cashCard);
            }
        }
    }
}
