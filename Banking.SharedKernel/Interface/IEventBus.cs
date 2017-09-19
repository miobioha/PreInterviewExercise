using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.SharedKernel.Interface
{
    public interface IEventBus
    {
        void Publish<T>(T command) where T : IEvent;
    }
}
