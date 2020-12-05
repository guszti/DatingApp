using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Extensions;
using DatingApp.API.Dtos;
using DatingApp.API.Factory;
using DatingApp.API.Helpers;
using DatingApp.API.ParamObject;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class MessageController : BaseController
    {
        private IMessageRepository messageRepository;

        private IMessageFactory messageFactory;

        private IUserRepository userRepository;

        public MessageController(
            IBaseRepository baseRepositoryInterface,
            IMapper mapper,
            IMessageRepository messageRepository,
            IMessageFactory messageFactory,
            IUserRepository userRepository
        )
            : base(baseRepositoryInterface, mapper)
        {
            this.messageRepository = messageRepository;
            this.messageFactory = messageFactory;
            this.userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            int sourceId = this.User.GetUserId();

            if (sourceId == createMessageDto.TargetUserId)
            {
                return BadRequest("You cannot send messages to yourself.");
            }

            var sourceUser = await this.userRepository.FindById(sourceId);
            var targetUser = await this.userRepository.FindById(createMessageDto.TargetUserId);

            if (sourceUser == null || targetUser == null)
            {
                return NotFound("Source or target user not found.");
            }

            var message = this.messageFactory.CreateWithUsers(sourceUser, targetUser, createMessageDto.Content);
            
            this.baseRepositoryInterface.AddNew(message);

            if (await this.baseRepositoryInterface.SaveAll())
            {
                return Created("", this.mapper.Map<MessageDto>(message));
            }

            return BadRequest("Failed to create message.");
        }

        [HttpGet]
        public async Task<Grid<MessageDto>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.UserId = this.User.GetUserId();

            var grid = await this.messageRepository.GetMessagesForUser(messageParams);
            
            Response.AddPaginationHeader(grid.Page, grid.Limit, grid.Total, grid.TotalPages);

            return grid;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<MessageDto>> GetMessageThread(int id)
        {
            var userId = this.User.GetUserId();

            return await this.messageRepository.GetMessageThread(userId, id);
        }
    }
}