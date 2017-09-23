using System;
using System.Threading.Tasks;

namespace Banking.SharedKernel
{
    public class ActionHelper
    {
        public static async Task ExecuteWithRetryAsync(Action execute, int retry, TimeSpan delay, Predicate<Exception> isTransient)
        {
            for (int currentRetry = 0; ;)
            {
                try
                {
                    execute();  

                    return;
                }
                catch (Exception ex)
                {
                    currentRetry++;

                    if (currentRetry >= retry || !isTransient(ex))
                    {
                        throw;
                    }
                }

                await Task.Delay(delay);
            }
        }
    }
}
