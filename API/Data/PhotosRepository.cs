using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class PhotosRepository:IPhotoRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public PhotosRepository(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<PhotoDto>> GetUnapprovedPhotosByUserName(string userName)
    {
        var user = this._dataContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        var query = this._dataContext.Photos.Where(a => a.AppUserId == user.Id && !a.IsApproved);
        var photos = await query.ProjectTo<PhotoDto>(_mapper.ConfigurationProvider).ToListAsync();
        return photos;    }

    public async Task<IEnumerable<PhotoDto>> GetUnapprovedPhotos()
    {
        var query = this._dataContext.Photos.Where(a => !a.IsApproved);
        var photos = await query.ProjectTo<PhotoDto>(_mapper.ConfigurationProvider).ToListAsync();
        return photos;    
    }

    public async Task<Photo> GetPhotoById(int photoId)
    {
        return await _dataContext.Photos.FindAsync(photoId);
    }

    public async Task RemovePhoto(int photoId)
    {
        var photo = await _dataContext.Photos.FindAsync(photoId);
        if(photo!=null) _dataContext.Photos.Remove(photo);
    }
}