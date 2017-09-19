using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Banking.SharedKernel.Test
{
    public class EventBusFixture
    {
        private EventBus _eventBus;

        [SetUp]
        public void GivenAEventBus()    
        {
            // Arrange
            _eventBus = new EventBus();
        }

        [Test]
        public void WhenEventIsPublish_ThenAllHandlersAreInvoked()
        {
            // Arrange
            var list = new List<string> { "FAIL", "FAIL", "FAIL" };
            _eventBus.RegisterHandler<EventBusTested>(_ => list[0] = "PASS");
            _eventBus.RegisterHandler<EventBusTested>(_ => list[1] = "PASS");
            _eventBus.RegisterHandler<EventBusTested>(_ => list[2] = "PASS");

            // Act
            _eventBus.Publish(new EventBusTested());

            // Assert
            Assert.That(list.TrueForAll(x => x == "PASS"));
        }


        [Test]
        public void WhenCommandIsSentAndNoHandlersHaveBeenRegistered_ShouldThrowInvalidOperationExcetion()
        {
            Assert.Throws<InvalidOperationException>(() => _eventBus.Publish(new EventBusTested()));
        }
    }
}
