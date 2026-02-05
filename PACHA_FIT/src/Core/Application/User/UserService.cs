using PACHA_FIT.Core.Application.User.Mappers;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.Dtos;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Core.Domain.User.Specifications;

namespace PACHA_FIT.Core.Application.User;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ResultDto<UserResponseDto>> GetUserAsync(UserSearchingRequest filter)
    {
        ISpecification<Domain.Entities.User> spec;

        if (filter.UserId.HasValue)
        {
            spec = new UserByIdSpecification(filter.UserId.Value);
        }
        else if (!string.IsNullOrEmpty(filter.Email))
        {
            spec = new UserByEmailSpecification(filter.Email);
        }
        else
        {
            return ResultDto<UserResponseDto>.Failure("Criterio de búsqueda inválido", ErrorCodes.BadRequest);
        }

        var user = await _userRepository.GetOneAsync(spec);
        
        if (user == null)
        {
            return ResultDto<UserResponseDto>.Failure("Usuario no encontrado", ErrorCodes.NotFound);
        }

        var response = UserMapper.UserToResponse(user);
        return ResultDto<UserResponseDto>.Success(response);
    }

    public async Task<ResultDto<string>> UpdateUser(UpdateProfileRequest body, string userId)
    {
        if (!int.TryParse(userId, out int id))
        {
             return ResultDto<string>.Failure("Id de usuario inválido", 400);
        }

        var user = await _userRepository.GetOneAsync(new UserByIdSpecification(id));

        if (user == null)
        {
            return ResultDto<string>.Failure("Usuario no encontrado", ErrorCodes.NotFound);
        }
            
        UserMapper.UpdateProfileFromRequest(body, user);

        await _userRepository.UpdateUser(user);
        
        return ResultDto<string>.Success("Usuario actualizado correctamente");
    }

    public async Task<ResultDto<string>> UpdateUser(UpdateUserRequest request, string userId)
    {
        if (!int.TryParse(userId, out int id))
        {
            return ResultDto<string>.Failure("Id de usuario inválido", 400);
        }

        var user = await _userRepository.GetOneAsync(new UserByIdSpecification(id));

        if (user == null)
        {
            return ResultDto<string>.Failure("Usuario no encontrado", ErrorCodes.NotFound);
        }
            
        UserMapper.UpdateProfileFromRequest(request, user);

        await _userRepository.UpdateUser(user);
        
        return ResultDto<string>.Success("Usuario actualizado correctamente");
    }
}