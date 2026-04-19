using System.ComponentModel.DataAnnotations;

namespace PACHA_FIT.Api;

public static class CustomObjectValidator
{
    public static void Validate(object model)
    {
        var context = new ValidationContext(model);
        Validator.ValidateObject(model, context, validateAllProperties: true);
    }
}