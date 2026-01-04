using System.ComponentModel.DataAnnotations;

namespace PACHA_FIT.Core.Domain.Shared;

public static class ObjectValidator
{
    public static void Validate(object model)
    {
        var context = new ValidationContext(model);
        Validator.ValidateObject(model, context, validateAllProperties: true);
    }
}