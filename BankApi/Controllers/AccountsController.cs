using System;
using System.Net;
using System.Threading.Tasks;
using Banking.Core.Commands;
using Banking.Core.Exceptions;
using Banking.Core.Services;
using Banking.SharedKernel;
using Banking.SharedKernel.Interface;
using System.Web.Mvc;
using System.Web;

namespace BankApi.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ICommandBus _commandBus;
        private readonly ITransactionServices _transactionServices;

        internal Action<HttpResponseBase, int> SetResponseStatusCode = (response, statusCode) => response.StatusCode = statusCode;

        public AccountsController(ITransactionServices transactionServices,
            ICommandBus commandBus)
        {
            _commandBus = commandBus;
            _transactionServices = transactionServices;
        }

        // POST: api/accounts/create
        [HttpPost]
        public async Task<JsonResult> CreateAccount(string owner, decimal initialAmount)
        {
            var accountId = await Task.Run(() =>
            {
                var command = new CreateAccount { BankId = 1, InitialAmount = initialAmount };
                _commandBus.Send(command);
                return command.AccountId;
            });

            SetResponseStatusCode(Response, (int) HttpStatusCode.Created);

            return new JsonResult
            {
                Data = new
                {
                    accountId = accountId
                }
            };
        }

        // POST: api/accounts/withdraw
        [HttpPut]
        public async Task<JsonResult> Withdraw(Guid cardId, string cardPin, decimal amount)
        {
            bool success = false;
            string status = "success";

            try
            {
                await ActionHelper.ExecuteWithRetryAsync(
                    () => _transactionServices.Withdraw(cardId, cardPin, amount),
                    5,
                    TimeSpan.Zero,
                    IsTransient);
                success = true;
            }
            catch (InvalidPinException)
            {
                SetResponseStatusCode(Response, (int) HttpStatusCode.BadRequest);
                status = "Invalid pin number";
            }
            catch (InsufficientFundsException)
            {
                SetResponseStatusCode(Response, (int) HttpStatusCode.PreconditionFailed);
                status = "Insufficient funds";
            }
            catch (ConcurrencyException)
            {
                // Abort; max retry reached but unsuccessful.
                SetResponseStatusCode(Response, (int) HttpStatusCode.InternalServerError);
                status = "Unknown";
            }

            return new JsonResult
            {
                Data = new
                {
                    success = success,
                    status = status
                }
            };
        }

        private bool IsTransient(Exception exception)
        {
            return exception is ConcurrencyException;
        }
    }
}
