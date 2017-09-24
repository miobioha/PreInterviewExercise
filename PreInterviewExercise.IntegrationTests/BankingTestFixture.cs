using Banking.Core.Aggregates;
using Banking.Core.ReadModel.Implementation;
using Banking.Core.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Vending.Core;
using Vending.Core.Exceptions;

namespace PreInterviewExercise.IntegrationTests
{
    [TestFixture]
    public class BankingTestFixture
    {   
        private Guid _accountId;
        private Guid _accountWithZeroBalanceId;

        private const int BankId = 1;
        private const string Pin = "1234";
        private const string BadPin = "0000";
        private const int CashCardCount = 2;
        private const string Endpoint = "http://localhost:46902/api/accounts/withdraw";

        private readonly List<Guid> _cashCards = new List<Guid>();
        private Guid _cashCardWithZeroBalance;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _accountId = TestHelper.CreateAccount(BankId, 1000.00m);           
            Debug.WriteLine("Account {0} created.", _accountId);
            _accountWithZeroBalanceId = TestHelper.CreateAccount(BankId, 0.00m);
            Debug.WriteLine("Account {0} created.", _accountWithZeroBalanceId);          
            for (int i = 0; i < CashCardCount; i++)
            {
                _cashCards.Add(TestHelper.CreateCashCard(_accountId, Pin));
            }
            _cashCardWithZeroBalance = TestHelper.CreateCashCard(_accountWithZeroBalanceId, Pin);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            DeleteAccount();
            DeleteCashCards();
        }

        [Test]
        public void WhenUserBuysSoftDrink_AccountBalanceIsUpdated()
        {
            var vendingMachine = new VendingMachine(new AccountServices(Endpoint));
            vendingMachine.LoadNewItemPile(new ItemPile(Item.CanBeverage, 25, 0.50m));

            var balance = GetAccount(_accountId).Balance;

            vendingMachine.VendItem(_cashCards[0], Pin);

            Assert.AreEqual(balance - 0.50m, GetAccount(_accountId).Balance);
            Assert.AreEqual(24, vendingMachine.ItemPile.Quantity);
        }

        [Test]
        public void WhenUserAttemptsToBuysSoftDrinkWithZeroBalanceOnAccount_ItemIsNotDispensed()
        {
            var vendingMachine = new VendingMachine(new AccountServices(Endpoint));
            vendingMachine.LoadNewItemPile(new ItemPile(Item.CanBeverage, 25, 0.50m));

            var balance = GetAccount(_accountWithZeroBalanceId).Balance;

            Assert.Throws<TransactionFailedException>(
                () => vendingMachine.VendItem(_cashCardWithZeroBalance, BadPin));
            Assert.AreEqual(balance, GetAccount(_accountWithZeroBalanceId).Balance);
            Assert.AreEqual(25, vendingMachine.ItemPile.Quantity);
        }

        [Test]
        public void WhenUserAttemptsToBuysSoftDrinkWithInvalidPin_ItemIsNotDispensed()
        {
            var vendingMachine = new VendingMachine(new AccountServices(Endpoint));
            vendingMachine.LoadNewItemPile(new ItemPile(Item.CanBeverage, 25, 0.50m));

            var balance = GetAccount(_accountId).Balance;

            Assert.Throws<TransactionFailedException>(() => vendingMachine.VendItem(_cashCards[0], BadPin));
            Assert.AreEqual(balance, GetAccount(_accountId).Balance);
            Assert.AreEqual(25, vendingMachine.ItemPile.Quantity);
        }

        [Test]
        public void WhenTwoUsersWithCashCardsLinkedToTheSameAccount_BuySoftDrinkAtTheSameTime_BothTransactionsAreSuccessful()
        {
            var vendingMachine1 = new VendingMachine(new AccountServices(Endpoint));
            var vendingMachine2 = new VendingMachine(new AccountServices(Endpoint));
            vendingMachine1.LoadNewItemPile(new ItemPile(Item.CanBeverage, 25, 0.50m));
            vendingMachine2.LoadNewItemPile(new ItemPile(Item.CanBeverage, 25, 0.50m));

            var balance = GetAccount(_accountId).Balance;

            Parallel.Invoke(
                () => vendingMachine1.VendItem(_cashCards[0], Pin),
                () => vendingMachine2.VendItem(_cashCards[1], Pin));

            Assert.AreEqual(balance - 1.00m, GetAccount(_accountId).Balance);
            Assert.AreEqual(24, vendingMachine1.ItemPile.Quantity);
            Assert.AreEqual(24, vendingMachine1.ItemPile.Quantity);
        }


        private void DeleteAccount()
        {
            var commandText = "DELETE FROM Accounts WHERE Id = @Id";
            var parameterId = new SqlParameter("@Id", _accountId);
            var connectionString = ConfigurationManager.ConnectionStrings["BankDbContext"].ConnectionString;
            int rows = ExecuteNonQuery(connectionString, commandText, CommandType.Text, parameterId);
        }

        private void DeleteCashCards()
        {
            foreach (var cashCardId in _cashCards)
            {
                var commandText = "DELETE FROM CashCards WHERE Id = @Id";
                var parameterId = new SqlParameter("@Id", cashCardId);
                var connectionString = ConfigurationManager.ConnectionStrings["BankDbContext"].ConnectionString;
                int rows = ExecuteNonQuery(connectionString, commandText, CommandType.Text, parameterId);
            }
        }

        private Account GetAccount(Guid id)
        {
            var accountRepository = new AccountRepository(new BankContext());
            return accountRepository.GetById(id);
        }

        public static Int32 ExecuteNonQuery(String connectionString, String commandText,
          CommandType commandType, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                { 
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
