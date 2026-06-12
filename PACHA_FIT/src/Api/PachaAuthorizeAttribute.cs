namespace PACHA_FIT.Api;

[AttributeUsage(AttributeTargets.Method)]
public class PachaAuthorizeAttribute : Attribute
{
    public string Roles { get; set; } = string.Empty;
}