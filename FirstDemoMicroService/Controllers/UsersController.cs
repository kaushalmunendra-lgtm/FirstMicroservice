using FirstDemoMicroService.Domain.Entities;
using FirstDemoMicroService.Domain.Repositories;
using FirstDemoMicroService.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstDemoMicroService.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController(IUserRepository userRepository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);
        return Ok(users.Select(ToResponse).ToArray());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);
        return user is null ? NotFound() : Ok(ToResponse(user));
    }

    private static UserResponse ToResponse(User user) =>
        new(user.Id, user.FullName, user.Email, user.CreatedAtUtc);
}
