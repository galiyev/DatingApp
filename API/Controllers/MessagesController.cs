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
    private readonly IUserRepository _userRepository;
    private readonly IMessagesRepository _messagesRepository;
    private readonly IMapper _mapper;

    public MessagesController(IUserRepository userRepository, IMessagesRepository messagesRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _messagesRepository = messagesRepository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult> CreateMessage(CreateMessageDto createMessageDto)
    {
        var username = User.GetUserName();
        if (username == createMessageDto.RecipientUserName.ToLower()) return BadRequest("You can't send message to yourself");

        var sender = await _userRepository.GetUserByUserName(username);
        var recipient = await _userRepository.GetUserByUserName(createMessageDto.RecipientUserName);

        if (recipient == null) return NotFound();

        var message = new Message()
        {
            Sender = sender,
            Recipient = recipient,
            SenderUserName = sender.UserName,
            RecipientUserName = recipient.UserName,
            Content = createMessageDto.Content
        };
        
        _messagesRepository.AddMessage(message);
        if (await _messagesRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));

        return BadRequest("Failed to send message");
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
    {
        messageParams.Username = User.GetUserName();
        var messages = await _messagesRepository.GetMessagesForUser(messageParams);
        Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages));
        return messages;
    }

    [HttpGet("thread/{username}")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
    {
        var currentUserName = User.GetUserName();
        return Ok(await _messagesRepository.GetMessageThread(currentUserName, username));
    }
}