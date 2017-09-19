using NUnit.Framework;
using System;

namespace Banking.SharedKernel.Test
{
    [TestFixture]
    public class CommandBusTestFixture
    {
        private CommandBus _commandBus;

        [SetUp]
        public void GivenACommandBus()
        {
            // Arrange
            _commandBus = new CommandBus();
        }

        [Test]
        public void WhenMoreThanOneHandlerIsRegisteredForTheSameCommand_ThenInvalidOperationIsThrown()
        {
            // Arrange
            _commandBus.RegisterHandler<TestCommand>(_ => { });

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() => _commandBus.RegisterHandler<TestCommand>(command => { }));
        }

        [Test]
        public void WhenCommandIsSent_ThenHandlerIsInvoked()
        {
            // Arrange
            var str = "FAIL";
            _commandBus.RegisterHandler<TestCommand>(_ => str = "PASS");

            // Act
            _commandBus.Send(new TestCommand());

            // Assert
            Assert.That(str == "PASS");
        }

        [Test]
        public void WhenCommandIsSentAndNoHandlersHaveBeenRegistered_ShouldThrowInvalidOperationExcetion()
        {
            Assert.Throws<InvalidOperationException>(() => _commandBus.Send(new TestCommand()));
        }
    }
}
