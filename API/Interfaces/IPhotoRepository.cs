using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IPhotoRepository
{
    Task<IEnumerable<PhotoDto>> GetUnapprovedPhotosByUserName(string userName);
    Task<IEnumerable<PhotoDto>> GetUnapprovedPhotos();
    Task<Photo> GetPhotoById(int photoId);  
    Task RemovePhoto(int photoId);
}