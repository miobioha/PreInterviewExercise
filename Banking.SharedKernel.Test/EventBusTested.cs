using Banking.SharedKernel.Interface;
using System;

namespace Banking.SharedKernel.Test
{
    class EventBusTested : IEvent
    {
        public EventBusTested()
        {
            Timestamp = DateTimeOffset.Now;

        }
        public DateTimeOffset Timestamp { get; private set; }
    }
}
