using System.ComponentModel.DataAnnotations;
using Banking.SharedKernel.Interface;
using System;
using System.Collections.Generic;

namespace Banking.SharedKernel
{
    public abstract class AggregateRoot : Entity<Guid>
    {
        private readonly List<IEvent> _changes = new List<IEvent>();

        public int Version { get; internal set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public IEnumerable<IEvent> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history)
            {
                ApplyChange(e, false);
            }
        }

        protected void ApplyChange(IEvent @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(IEvent @event, bool isNew)
        {
            this.AsDynamic().Apply(@event);
            if (isNew)
            {
                _changes.Add(@event);
            }
        }
    }
}
