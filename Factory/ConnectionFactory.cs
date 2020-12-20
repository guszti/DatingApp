using DatingApp.API.Model;

namespace DatingApp.API.Factory
{
    public class ConnectionFactory : BaseFactory, IConnectionFactory
    {
        public Connection CreateForGroup(Group group, string connectionId)
        {
            var connection = this.CreateNew<Connection>();

            connection.ConnectionId = connectionId;
            group.Connections.Add(connection);

            return connection;
        }
    }
}