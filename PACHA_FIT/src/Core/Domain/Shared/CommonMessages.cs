namespace PACHA_FIT.Core.Domain.Shared;

public static class CommonMessages
{
    public const string InvalidRequestBody = "La petición no contiene un cuerpo válido o está vacío";
    public const string UnauthorizedAccess = "No tienes permisos para realizar esta acción o el token es inválido";
    public const string UserNotFound = "Usuario no encontrado";
    public const string UserAlreadyExists = "El usuario ya existe";
    public const string InvalidId = "El identificador proporcionado no es válido";
    public const string InternalServerError = "Se produjo un error interno en el servidor";

    public static class Validation
    {
        public const string PasswordTooWeak = "La contraseña debe tener al menos 8 caracteres, una letra mayúscula y un número";
        public const string ValidationError = "Error de validación en los datos";
    }

    public static class Auth
    {
        public const string RegistrationSuccess = "Usuario registrado correctamente";
        public const string InvalidCredentials = "Credenciales incorrectas";
        public const string PasswordChangedSuccess = "Contraseña actualizada correctamente";
    }
}
