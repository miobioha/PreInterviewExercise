using System;
using BankApi.Controllers;
using Banking.Core.Commands;
using Banking.Core.Services;
using Banking.SharedKernel.Interface;
using Moq;
using NUnit.Framework;
using System.Web;

namespace BankApi.Test.Controllers
{
    [TestFixture]
    public class AccountsControllerTestFixture
    {
        private AccountsController _controller;
        private Mock<ITransactionServices> _transactionServices;
        private Mock<ICommandBus> _commandBus;
            
        [SetUp]
        public void GivenController()
        {
            _transactionServices = new Mock<ITransactionServices>();
            _commandBus = new Mock<ICommandBus>();
            _controller =  new AccountsController(_transactionServices.Object, _commandBus.Object);
            _controller.SetResponseStatusCode = SetHttpResponseCode;
        }

        [Test]
        public void WhenCreateAccountIsCalled_SendsCreateAccountCommandToBus()
        {
            _controller.CreateAccount("", 1000.00m).Wait();

            _commandBus.Verify(bus => bus.Send(It.IsAny<CreateAccount>()));
        }

        [Test]
        public void WhenWithdrawIsCalled_CallsWithdrawOnTransaction()
        {
            _controller.Withdraw(Guid.NewGuid(), "1234", 1000.00m).Wait();

            _transactionServices.Verify(svc => svc.Withdraw(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<decimal>()));
        }

        private void SetHttpResponseCode(HttpResponseBase notUsed, int notUsedEither)
        {
        }
    }
}
