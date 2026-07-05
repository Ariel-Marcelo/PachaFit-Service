namespace PACHA_FIT.Infrastructure.Api;

[AttributeUsage(AttributeTargets.Method)]
public class PachaAuthorizeAttribute : Attribute
{
    public string Roles { get; set; } = string.Empty;
}