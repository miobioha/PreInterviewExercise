using System;
using Banking.Core.Commands;
using Banking.Core.Handlers;
using Banking.Core.ReadModel.Implementation;
using Banking.Core.Repositories;
using Banking.Core.Services;
using Banking.SharedKernel;
using Banking.SharedKernel.Interface;
using Microsoft.Practices.Unity;

namespace BankApi.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<BankContext>(new TransientLifetimeManager());

            container.RegisterType<ICommandBus, CommandBus>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<ICashCardRepository, CashCardRepository>();
            container.RegisterType<ICashCardRepositoryFactory, CashCardRepositoryFactory>();
            container.RegisterType<IAccountRepositoryFactory, AccountRepositoryFactory>();
            container.RegisterType<ITransactionServices, TransactionServices>();
            container.RegisterType<IHashingService, Sha256HashingService>(new ContainerControlledLifetimeManager());

            var accountCommandHandlers = container.Resolve<AccountCommandHandlers>();
            var commandBus = container.Resolve<CommandBus>();
            commandBus.RegisterHandler<CreateAccount>(accountCommandHandlers.Handle);
            commandBus.RegisterHandler<WithdrawFromAccount>(accountCommandHandlers.Handle);
        }
    }
}
