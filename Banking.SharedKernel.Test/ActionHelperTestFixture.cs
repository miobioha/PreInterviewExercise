using NUnit.Framework;
using System;

namespace Banking.SharedKernel.Test
{
    [TestFixture]
    public class ActionHelperTestFixture
    {
        [Test]
        public void WhenActionIsExecutedWithTransientFailure_RepeatsSpecifiedRetryCountAndThrowsException()
        {
            int retry = 0;

            Assert.Throws<AggregateException>(() => ActionHelper.ExecuteWithRetryAsync(
                () =>
                {
                    retry++;
                    throw new Exception();
                },
                5,
                TimeSpan.Zero,
                ex => true).Wait());

            Assert.AreEqual(5, retry);
        }

        [Test]
        public void WhenActionIsExecutedWithNoFailure_ExecutesOnce()
        {
            int retry = 0;

            ActionHelper.ExecuteWithRetryAsync(
                () => retry++,
                5,
                TimeSpan.Zero,
                ex => true).Wait();

            Assert.AreEqual(1, retry);
        }
    }
}
