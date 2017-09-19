using NUnit.Framework;
using System;

namespace Banking.SharedKernel.Test
{
    [TestFixture]
    public class GuardTests
    {
        [Test]
        public void ThrowsException_WhenPinIsNotANumber()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.ForInvalidPin("TEST", "notUsed"));
        }

        [Test]
        public void ThrowsException_WhenPinIsLessThanFourDigits()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.ForInvalidPin("123", "notUsed"));
        }

        [Test]
        public void ThrowsException_WhenPinIsMoreThanSixDigits()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.ForInvalidPin("1234567", "notUsed"));
        }

        [Test]
        public void DoesNotThrowException_WhenPinIsExactlyFourDigits()
        {
            Assert.DoesNotThrow(() => Guard.ForInvalidPin("1234", "notUsed"));
        }

        [Test]
        public void ThrowsException_WhenStringIsNull()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.ForNullOrEmpty(null, "notUsed"));
        }

        [Test]
        public void ThrowsException_WhenStringIsEmpty()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.ForNullOrEmpty(string.Empty, "notUsed"));
        }
    }
}
