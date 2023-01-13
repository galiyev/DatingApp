﻿using API.DTOs;
using API.Entities;
using API.Helpers;
using CloudinaryDotNet;

namespace API.Interfaces;

public interface ILikesRepository
{
    Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);
    Task<AppUser> GetUserWithLikes(int userId);
    Task<PagedList<LIkeDto>> GetUserLikes(LikesParams likesParams);
}