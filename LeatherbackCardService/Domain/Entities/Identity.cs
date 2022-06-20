using Shared.Exceptions;

namespace Domain.Entities
{
    public record Identity<T>
    {
        public Identity() { }
        private T Id { get; init; }
        public Identity(T id)
        {
            if (default(T).Equals(id))
                throw new InvalidEntityException("Id is invalid");
            Id = id;
        }
    }
}