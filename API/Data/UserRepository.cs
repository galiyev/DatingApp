using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }
    
    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }

    public async Task<bool> SaveAllAsync()
    {
        return await this._context.SaveChangesAsync()>0;
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await _context.Users
            .Include(x=>x.Photos)
            .ToListAsync();
    }

    public async Task<AppUser> GetUserById(int id)
    {
        return await _context.Users
            .FindAsync(id).ConfigureAwait(false);
    }

    public async Task<AppUser> GetUserByUserName(string username)
    {
        
        return await _context.Users
            .Include(x=>x.Photos)
            .SingleOrDefaultAsync(x=>x.UserName==username);
    }
}