using API.Interfaces;
using AutoMapper;

namespace API.Data;

public class UnitOfWork:IUnitOfWork
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UnitOfWork(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }


    public IUserRepository UserRepository => new UserRepository(_dataContext, _mapper);
    public IMessagesRepository MessagesRepository => new MessagesRepository(_dataContext, _mapper);
    public ILikesRepository LikesRepository => new LikesRepository(_dataContext);

    public IPhotoRepository PhotoRepository => new PhotosRepository(_dataContext, _mapper);

    public async Task<bool> Complete()
    {
        try
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public bool HasChanges()
    {
        return _dataContext.ChangeTracker.HasChanges();
    }
}