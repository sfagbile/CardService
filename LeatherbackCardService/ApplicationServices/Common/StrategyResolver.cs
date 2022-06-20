using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationServices.Interfaces;

namespace ApplicationServices.Common
{
    public class StrategyResolver<T> : IStrategyResolver<T> where T : IStrategyProcessor
    {
        private readonly IEnumerable<T> _availableProcessor;

        public StrategyResolver(IEnumerable<T> availableProcessor)
        {
            _availableProcessor = availableProcessor;
        }

        public T GetService(string providerName)
        {
            var providerService = _availableProcessor.FirstOrDefault(x => x.ResolverValue == providerName);
            if (providerService == null)
            {
                throw new NotImplementedException($"Provider processors has not been implemented.");
            }
            return providerService;
        }
    }
}