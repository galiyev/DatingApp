﻿using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser> GetUserById(int id);
    Task<AppUser> GetUserByUserName(string username);

    Task<MemberDto> GetMemberAsync(string username);
    Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
}                                             