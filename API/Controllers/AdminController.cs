using System.Reflection;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AdminController:BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _uow;

    public AdminController(UserManager<AppUser> userManager, IUnitOfWork uow)
    {
        _userManager = userManager;
        _uow = uow;
    }
    
    
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("users-with-roles")]
    public async Task<ActionResult> GetUsersWithRoles()
    {
        var users = await _userManager.Users
            .OrderBy(u => u.UserName)
            .Select(u => new
            {
                u.Id,
                UserName = u.UserName,
                Roles = u.UserRoles.Select(r=>r.Role.Name).ToList()
            }).ToListAsync();
        return Ok(users);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("edit-roles/{username}")]
    public async Task<ActionResult> EditRoles(string username, [FromQuery]string roles)
    {
        if (string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role");
        var selectedRoles = roles.Split(",").ToArray();

        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return NotFound();
        
        var userRoles = await _userManager.GetRolesAsync(user);

        var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

        if (!result.Succeeded) return BadRequest("Failed to add to roles");

        result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

        if (!result.Succeeded) return BadRequest("Failed to remove from roles");

        return Ok(await _userManager.GetRolesAsync(user));
    }

    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpGet("photos-to-moderate")]
    public ActionResult GetPhotosForModeration()
    {
        return Ok("Admins or moderators can see this");
    }
    
    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpGet("photos-to-approval-by-username/{username}")]
    public ActionResult GetPhotosForApprovalByUserName(string username)
    {
        var photosToApproval =  this._uow.PhotoRepository.GetUnapprovedPhotosByUserName(username);
        return Ok(photosToApproval);
    }
    
    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpGet("photos-to-approval")]
    public async Task<ActionResult> GetPhotosForApproval()
    {
        var photosToApproval =  await this._uow.PhotoRepository.GetUnapprovedPhotos();
        return Ok(photosToApproval.ToList());
    }
    
    
    [Authorize(Policy = "ModeratePhotoRole")]   
    [HttpPut("approve-photo/{photoId}")]
    public async Task<ActionResult> ApprovePhoto(int photoId)
    {
        var photoToApprove = await this._uow.PhotoRepository.GetPhotoById(photoId);
        if (photoToApprove == null) return NotFound();
        photoToApprove.IsApproved = true;

        var member = await this._uow.UserRepository.GetUserById(photoToApprove.AppUserId);
        var memberwithPhoto = await this._uow.UserRepository.GetUserByUserName(member.UserName);
        
        if (!memberwithPhoto.Photos.Any(p => p.IsMain))
        {
            photoToApprove.IsMain = true;
        }
        
        if (await this._uow.Complete()) return Ok();
        return BadRequest("Failed to approve photo");
    }
    
    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpPut("reject-photo/{photoId}")]
    public async Task<ActionResult> RejectPhoto(int photoId)
    {
        await this._uow.PhotoRepository.RemovePhoto(photoId);
        if (_uow.HasChanges())
        {
            if (await this._uow.Complete()) return Ok();
        }
        return BadRequest("Failed to reject photo");
    }

}