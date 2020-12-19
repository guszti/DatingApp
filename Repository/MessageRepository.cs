using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.API.Dtos;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using DatingApp.API.ParamObject;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class MessageRepository : BaseRepository, IMessageRepository
    {
        public MessageRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Grid<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = this.context.Message
                .OrderByDescending(m => m.CreatedAt)
                .AsQueryable();

            query = messageParams.Status switch
            {
                "Inbox" => query.Where(m => m.TargetId == messageParams.UserId && !m.TargetDeleted),
                "Sent" => query.Where(m => m.SourceId == messageParams.UserId && !m.SourceDeleted),
                _ => query.Where(m => m.TargetId == messageParams.UserId && m.SeenAt == null)
            };

            var messages = query.ProjectTo<MessageDto>(this.mapper.ConfigurationProvider);

            return await Grid<MessageDto>.CreateGridAsync(messages, messageParams.Page, messageParams.Limit);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(int sourceId, int targetId)
        {
            var messages = await this.context.Message
                .Include(m => m.Source)
                .ThenInclude(s => s.Photos)
                .Include(m => m.Target)
                .ThenInclude(t => t.Photos)
                .OrderByDescending(m => m.CreatedAt)
                .Where(
                    m =>
                        m.SourceId == sourceId
                        && !m.SourceDeleted
                        && m.TargetId == targetId
                        && !m.TargetDeleted
                        || m.SourceId == targetId
                        && m.TargetId == sourceId
                )
                .ToListAsync();

            var unread = messages.Where(m => m.SeenAt == null).ToList();

            if (unread.Any())
            {
                foreach (var message in unread)
                {
                    message.SeenAt = DateTime.UtcNow;
                }

                await this.SaveAll();
            }

            return this.mapper.Map<IEnumerable<MessageDto>>(messages);
        }
    }
}