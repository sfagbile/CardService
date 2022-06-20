using AutoMapper;

namespace ApplicationServices.AutomapperConfiguration
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}