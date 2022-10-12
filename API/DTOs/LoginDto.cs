namespace API.DTOs;

public class LoginDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class UserDto
{
    public string UserName { get; set; }    
    public string Token { get; set; }
}