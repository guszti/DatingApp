using System.Threading.Tasks;

namespace DatingApp.API.Repository
{
    public interface IUnitOfWork
    {
        IBaseRepository BaseRepository { get; }
        
        IUserRepository UserRepository { get; }
        
        IUserLikeRepository UserLikeRepository { get; }
        
        IMessageRepository MessageRepository { get; }
        
        IConnectionRepository ConnectionRepository { get; }
        
        IGroupRepository GroupRepository { get; }

        Task<bool> SaveChangesAsync();

        bool HasChanges();
    }
}