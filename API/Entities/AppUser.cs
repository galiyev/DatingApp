using System.ComponentModel;

namespace API.Entities;

public class AppUser
{
    // going to be primary
    [DisplayName("User Id")]
    public int Id { get; set; }
    
    public string? UserName { get; set; }

    public byte[] PasswordHash { get; set; }
    
    public byte[] PasswordSalt { get; set; }

}