namespace ApplicationServices.Interfaces
{
    public interface IStrategyResolver<out T>
    {
        T GetService(string providerName);
    }
}