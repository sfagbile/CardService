namespace ApplicationServices.Interfaces
{
    public interface ICommonHub
    {
        bool JoinGroup(string groupName, string connectionId);
        string GetConnectionIdByCustomerId(string groupName);
        bool RemoveFromGroup(string groupName, string connectionId);
        string GetGroupName(string connectionId);
    }
}