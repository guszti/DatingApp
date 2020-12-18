namespace DatingApp.API.Model
{
    public class Connection : IConnection
    {
        public string ConnectionId { get; set; }

        public string GroupName { get; set; }

        public string Username { get; set; }
    }
}