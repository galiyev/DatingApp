namespace API.DTOs;

public class PhotoForApprovalDto
{
    public int PhotoId { get; set; }
    public string Url { get; set; }
    public string Username { get; set; }
    public bool isApproved { get; set; }
}