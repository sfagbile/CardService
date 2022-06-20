namespace ApplicationServices.Interfaces
{
    public interface ICardIssuerResolver<out T>
    {
        T GetService(string country);
    }
}