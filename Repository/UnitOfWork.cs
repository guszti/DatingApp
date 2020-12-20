using System;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;

namespace DatingApp.API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMapper mapper;

        private readonly DataContext context;

        public UnitOfWork(IMapper mapper, DataContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public IBaseRepository BaseRepository => new BaseRepository(context, mapper);

        public IUserRepository UserRepository => new UserRepository(context, mapper);

        public IUserLikeRepository UserLikeRepository => new UserLikeRepository(context, mapper);

        public IMessageRepository MessageRepository => new MessageRepository(context, mapper);

        public IConnectionRepository ConnectionRepository => new ConnectionRepository(context, mapper);

        public IGroupRepository GroupRepository => new GroupRepository(context, mapper);
        
        public async Task<bool> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return this.context.ChangeTracker.HasChanges();
        }
    }
}