using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Exceptions;

namespace Shared
{
    public abstract class Entity<T>
    {
        public T Id { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime? DeletedDate { get; set; }   
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        private readonly List<object> _events;

        protected Entity()
        {
            Created = DateTime.UtcNow;
            IsDeleted = false;
            LastModified = DateTime.UtcNow;
        }
        protected void Apply(object @event)
        {
            When(@event);
            _events.Add(@event);
        }
        protected abstract void When(object @event);
        public IEnumerable<object> GetChanges() => _events.AsEnumerable();
        public void ClearChanges() => _events.Clear();
        
        protected Entity(T id)
        {
            if (default(T).Equals(id))
                throw new InvalidEntityException("Id is invalid");
            Id = id;
            Created = DateTime.UtcNow;
            IsDeleted = false;
            LastModified = DateTime.UtcNow;
            _events = new List<object>();
        }
    }
}