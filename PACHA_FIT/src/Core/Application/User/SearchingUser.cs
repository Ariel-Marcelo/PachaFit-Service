namespace PACHA_FIT.Core.Application.User;

public record SearchingUser
{
    public string Email {get; init;}
    public string Role { get; init; }
}