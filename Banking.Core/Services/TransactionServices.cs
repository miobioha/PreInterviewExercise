using System;
using Banking.Core.Commands;
using Banking.Core.Exceptions;
using Banking.Core.Repositories;
using Banking.SharedKernel.Interface;

namespace Banking.Core.Services
{
    public class TransactionServices : ITransactionServices
    {
        private readonly ICommandBus _commandBus;
        private readonly IHashingService _hashingService;
        private readonly ICashCardRepository _cashCardRepository;

        public TransactionServices(ICashCardRepository cashCardRepository,            
            IHashingService hashingService,
            ICommandBus commandBus)
        {
            _cashCardRepository = cashCardRepository;
            _commandBus = commandBus;
            _hashingService = hashingService;
        }

        public void Withdraw(Guid cardId, string pin, decimal amount)
        {
            var cashCard = _cashCardRepository.GetById(cardId);
            
            var pinHash = _hashingService.Hash(pin);
            if (cashCard.PinHash != pinHash)
            {
                throw new InvalidPinException();
            }

            _commandBus.Send(new WithdrawFromAccount {Id = cashCard.AccountId, Amount = amount});
        }
    }
}
