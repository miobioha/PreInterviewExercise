using System;
using System.Text.RegularExpressions;

namespace Banking.SharedKernel
{
    public static class Guard
    {
        private static readonly Regex PinNumber = new Regex("^[0-9]{4}$");

        public static void ForLessEqualZero(int value, string parameterName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        public static void ForLessThanAmount(decimal value, decimal amount, string parameterName)
        {
            if (value < amount)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        public static void ForNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        public static void ForInvalidPin(string value, string parameterName)
        {
            if (!PinNumber.IsMatch(value))
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }
    }
}
