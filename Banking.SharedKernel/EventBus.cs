using Banking.SharedKernel.Interface;
using System;
using System.Collections.Generic;

namespace Banking.SharedKernel
{
    public class EventBus
    {
        private readonly Dictionary<Type, List<Action<IEvent>>> _handlers = new Dictionary<Type, List<Action<IEvent>>>();

        public void RegisterHandler<T>(Action<T> handler) where T : IEvent
        {
            List<Action<IEvent>> handlers;
            if (!_handlers.TryGetValue(typeof(T), out handlers))
            {
                _handlers.Add(typeof(T), handlers = new List<Action<IEvent>>());
            }

            handlers.Add(@event => handler((T)@event));
        }

        public void Publish<T>(T @event) where T : IEvent
        {
            List<Action<IEvent>> handlers;
            if (_handlers.TryGetValue(typeof(T), out handlers))
            {
                handlers.ForEach(handler => handler(@event));
            }
            else
            {
                throw new InvalidOperationException("No handlers registered.");
            }
        }
    }
}
