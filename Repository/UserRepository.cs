using DatingApp.API.Data;

namespace DatingApp.API.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }
    }
}