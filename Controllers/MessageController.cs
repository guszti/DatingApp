using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Extensions;
using DatingApp.API.Dtos;
using DatingApp.API.Factory;
using DatingApp.API.Helpers;
using DatingApp.API.Model;
using DatingApp.API.ParamObject;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class MessageController : BaseController
    {
        private IMessageFactory messageFactory;

        public MessageController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMessageFactory messageFactory
        )
            : base(unitOfWork, mapper)
        {
            this.unitOfWork = unitOfWork;
            this.messageFactory = messageFactory;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            int sourceId = this.User.GetUserId();

            if (sourceId == createMessageDto.TargetUserId)
            {
                return BadRequest("You cannot send messages to yourself.");
            }

            var sourceUser = await this.unitOfWork.UserRepository.FindById(sourceId);
            var targetUser = await this.unitOfWork.UserRepository.FindById(createMessageDto.TargetUserId);

            if (sourceUser == null || targetUser == null)
            {
                return NotFound("Source or target user not found.");
            }

            var message = this.messageFactory.CreateWithUsers(sourceUser, targetUser, createMessageDto.Content);

            this.unitOfWork.BaseRepository.AddNew(message);

            if (await this.unitOfWork.SaveChangesAsync())
            {
                return Created("", this.mapper.Map<MessageDto>(message));
            }

            return BadRequest("Failed to create message.");
        }

        [HttpGet]
        public async Task<Grid<MessageDto>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.UserId = this.User.GetUserId();

            var grid = await this.unitOfWork.MessageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(grid.Page, grid.Limit, grid.Total, grid.TotalPages);

            return grid;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<MessageDto>> GetMessageThread(int id)
        {
            var userId = this.User.GetUserId();

            var messageThread = await this.unitOfWork.MessageRepository.GetMessageThread(userId, id);

            if (this.unitOfWork.HasChanges())
            {
                await this.unitOfWork.SaveChangesAsync();
            }

            return messageThread;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var userId = this.User.GetUserId();
            var message = await this.unitOfWork.MessageRepository.FindById<Message>(id);

            if (message == null)
            {
                return NotFound("Message not found.");
            }

            if (message.SourceId == userId)
            {
                message.SourceDeleted = true;
            }

            if (message.TargetId == userId)
            {
                message.TargetDeleted = true;
            }

            if (message.SourceDeleted && message.TargetDeleted)
            {
                this.unitOfWork.MessageRepository.Remove(message);
            }

            if (await this.unitOfWork.SaveChangesAsync())
            {
                return Ok();
            }

            return BadRequest("Failed to delete message.");
        }
    }
}