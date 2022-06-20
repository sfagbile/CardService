using System;
using System.Linq;
using System.Reflection;
using ApplicationServices.Card.Model;
using AutoMapper;
using Domain.Entities.Cards;

namespace ApplicationServices.AutomapperConfiguration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
            //configure mappings here

            CreateMap<Domain.Entities.Cards.Card, GetCardViewModel>();

            /*CreateMap<AddIndividualEndUserCommand, IndividualEndUserViewModelRequest>();
            CreateMap<AddCooperateEndUserCommand, CompanyEndUserViewModelRequest>();
            CreateMap<IssuePhysicalCardCommand, PhysicalCardRequestViewModel>();
            CreateMap<CardDeliveryAddressViewModel, CardDeliveryAddress>(); */
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod("Mapping")
                                 ?? type.GetInterface("IMapFrom`1").GetMethod("Mapping");

                methodInfo?.Invoke(instance, new object[] {this});
            }
        }
    }
}