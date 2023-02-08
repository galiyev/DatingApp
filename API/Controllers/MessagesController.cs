using System.ComponentModel;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MessagesController: BaseApiController
{
    // private readonly IUserRepository _userRepository;
    // private readonly IMessagesRepository _messagesRepository;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MessagesController(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult> CreateMessage(CreateMessageDto createMessageDto)
    {
        var username = User.GetUserName();
        if (username == createMessageDto.RecipientUserName.ToLower()) return BadRequest("You can't send message to yourself");

        var sender = await _uow.UserRepository.GetUserByUserName(username);
        var recipient = await _uow.UserRepository.GetUserByUserName(createMessageDto.RecipientUserName);

        if (recipient == null) return NotFound();

        var message = new Message()
        {
            Sender = sender,
            Recipient = recipient,
            SenderUserName = sender.UserName,
            RecipientUserName = recipient.UserName,
            Content = createMessageDto.Content
        };
        
        _uow.MessagesRepository.AddMessage(message);
        if (await _uow.Complete()) return Ok(_mapper.Map<MessageDto>(message));

        return BadRequest("Failed to send message");
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
    {
        messageParams.Username = User.GetUserName();
        var messages = await _uow.MessagesRepository.GetMessagesForUser(messageParams);
        Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages));
        return messages;
    }

    
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        var userName = User.GetUserName();
        var message = await _uow.MessagesRepository.GetMessage(id);

        if (message.SenderUserName != userName && message.RecipientUserName!=userName)
        {
            return Unauthorized();
        }

        if (message.SenderUserName == userName)  message.SenderDeleted = true;
        if (message.RecipientUserName == userName) message.RecipientDeleted = true;

        if (message.SenderDeleted && message.RecipientDeleted)
        {
            _uow.MessagesRepository.DeleteMessage(message);
        }

        if (await _uow.Complete()) return Ok();
        return  BadRequest("Problem deleting the message");
    }
}