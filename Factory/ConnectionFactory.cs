using DatingApp.API.Model;

namespace DatingApp.API.Factory
{
    public class ConnectionFactory : BaseFactory, IConnectionFactory
    {
        public Connection CreateForGroup(Group group)
        {
            var connection = this.CreateNew<Connection>();

            group.Connections.Add(connection);

            return connection;
        }
    }
}