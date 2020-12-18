namespace DatingApp.API.Model
{
    public interface IConnection
    {
        string ConnectionId { get; set; }
        
        string GroupName { get; set; }
        
        string Username { get; set; }
    }
}