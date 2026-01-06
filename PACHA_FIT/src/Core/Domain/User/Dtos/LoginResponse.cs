namespace PACHA_FIT.Core.Domain.User.Dtos;

public record LoginResponse
{
    public string Email { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    
    public string Token { get; init; } = string.Empty;
    
    public string RoleName { get; init; } = string.Empty;
}