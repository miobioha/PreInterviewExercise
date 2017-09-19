using Banking.SharedKernel.Interface;
using System;
using System.Collections.Generic;

namespace Banking.SharedKernel
{
    public class CommandBus : ICommandBus
    {
        private readonly Dictionary<Type, Action<ICommand>> _handlers = new Dictionary<Type, Action<ICommand>>();

        public void RegisterHandler<T>(Action<T> handler) where T : ICommand
        {
            if(_handlers.ContainsKey(typeof(T)))
            {
                throw new InvalidOperationException("Cannot register more than one handler for the same command type.");
            }

            _handlers.Add(typeof(T), command => handler((T)command));
        }

        public void Send<T>(T command) where T: ICommand
        {
            Action<ICommand> handler;

            if (_handlers.TryGetValue(typeof(T), out handler))
            {
                handler(command);
            }
            else
            {
                throw new InvalidOperationException("No handlers registered.");
            }
        }
    }
}
