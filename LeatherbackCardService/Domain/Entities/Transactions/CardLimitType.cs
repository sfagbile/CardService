using System;
using Shared;
using Shared.BaseResponse;

namespace Domain.Entities.Transactions
{
    public class CardLimitType : Entity<Guid>
    {
        private CardLimitType(Guid id, string type, string description)
        {
            Id = id;
            Type = type;
            Description = description;
        }
        
        public string Type { get; set; }
        public string Description { get; set; }
        
        public static Result<CardLimitType> CreateInstance(string type, string description)
        {
            if (type == default)
                return Result.Fail<CardLimitType>($"{nameof(type)} is invalid");

            if (description == default)
                return Result.Fail<CardLimitType>($"{nameof(description)} is invalid");
            
            return Result.Ok(new CardLimitType(Guid.NewGuid() , type, description));
        } 

        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }
    }
}