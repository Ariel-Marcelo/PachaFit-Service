namespace PACHA_FIT.Core.Domain.User.Dtos;

public class UserSearchingRequest
{
    public int? UserId { get; set; }
    
    public string? Email { get; set; }
    
    public string? UserName { get; set; }
}