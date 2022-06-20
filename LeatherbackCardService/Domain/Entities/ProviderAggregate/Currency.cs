using System;
using Shared;
using Shared.BaseResponse;
using Shared.Exceptions;

namespace Domain.Entities.ProviderAggregate
{
    public class Currency : Entity<Guid>
    {
        private Currency()
        {
        }

        private Currency(Guid id, string name, string code, string symbol)
        {
            Name = name;
            Code = code;
            Symbol = symbol;
        }

        public string Name { get; set; }

        public string Code { get; set; }

        /// <summary>
        /// Unicode symbol of country e.g. £, $, etc
        /// </summary>
        public string Symbol { get; set; }

        public bool IsBaseCurrency { get; set; }

        public bool IsActive { get; set; }

        public static Result<Currency> Create(Guid id, string name, string code, string symbol)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidEntityException($"{nameof(name)} can not be null or empty");

            if (string.IsNullOrEmpty(symbol))
                throw new InvalidEntityException($"{nameof(symbol)} can not be null or empty");

            if (string.IsNullOrEmpty(code))
                throw new InvalidEntityException($"{nameof(code)} can not be null or empty");

            return Result.Ok(new Currency(id, name, code, symbol)
            {
                IsActive = true,
                IsBaseCurrency = true
            });
        }

        protected override void When(object @event)
        {
            throw new System.NotImplementedException();
        }
    }
}