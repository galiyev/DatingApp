using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikesController:BaseApiController
{
    private readonly IUnitOfWork _uow;
   
    public LikesController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpPost("{username}")]
    public async Task<ActionResult> AddLike(string userName)
    {
        var sourceUserId = User.GetUserId(); 
        var likeUser = await _uow.UserRepository.GetUserByUserName(userName);
        var sourceUser = await _uow.LikesRepository.GetUserWithLikes(sourceUserId);

        if (likeUser == null) return NotFound();

        if (sourceUser.UserName == userName) return BadRequest("You cannot like yourself");

        var userLike = await _uow.LikesRepository.GetUserLike(sourceUserId, likeUser.Id);

        if (userLike != null) return BadRequest("You already liked this user");
        userLike = new UserLike()
        {
            SourceUserId = sourceUser.Id,
            TargetUserId = likeUser.Id
        };
        
        sourceUser.LikedUsers.Add(userLike);
        if (await _uow.Complete()) return Ok();
        return BadRequest("Failed to like user");
    }

    public async Task<ActionResult<PagedList<LIkeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
    {
        likesParams.UserId = User.GetUserId();
        var users = await _uow.LikesRepository.GetUserLikes(likesParams);
        Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, 
            users.PageSize, 
            users.TotalCount, 
            users.TotalPages));
        return Ok(users);
    }
    
}