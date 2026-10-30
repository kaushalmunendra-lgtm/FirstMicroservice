using FirstDemoMicroService.Controllers;
using FirstDemoMicroService.Domain.Entities;
using FirstDemoMicroService.Domain.Repositories;
using FirstDemoMicroService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace FirstDemoMicroService.Tests.Controllers;

public sealed class UsersControllerTests
{
    [Fact]
    public async Task GetById_ReturnsResponseDto()
    {
        var user = new User
        {
            Id = 7,
            FullName = "Alex Morgan",
            Email = "alex.morgan@example.com",
            CreatedAtUtc = DateTime.UtcNow
        };
        var controller = new UsersController(new StubUserRepository([user]));

        var result = await controller.GetById(user.Id, CancellationToken.None);

        var response = Assert.IsType<UserResponse>(Assert.IsType<OkObjectResult>(result.Result).Value);
        Assert.Equal(user.Id, response.Id);
        Assert.Equal(user.FullName, response.FullName);
        Assert.Equal(user.Email, response.Email);
    }

    [Fact]
    public async Task GetById_ReturnsNotFoundWhenUserDoesNotExist()
    {
        var controller = new UsersController(new StubUserRepository([]));

        var result = await controller.GetById(42, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetAll_ReturnsResponseDtos()
    {
        var controller = new UsersController(new StubUserRepository(
        [
            new User { Id = 1, FullName = "Alex Morgan", Email = "alex@example.com" },
            new User { Id = 2, FullName = "Sam Taylor", Email = "sam@example.com" }
        ]));

        var result = await controller.GetAll(CancellationToken.None);

        var responses = Assert.IsAssignableFrom<IReadOnlyList<UserResponse>>(
            Assert.IsType<OkObjectResult>(result.Result).Value);
        Assert.Equal(2, responses.Count);
    }

    [Fact]
    public async Task SwaggerDocument_ContainsBothUserEndpoints()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddControllers().AddApplicationPart(typeof(UsersController).Assembly);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        await using var app = builder.Build();
        app.MapControllers();

        var document = app.Services.GetRequiredService<ISwaggerProvider>().GetSwagger("v1");

        Assert.Contains("/api/users", document.Paths.Keys);
        Assert.Contains("/api/users/{id}", document.Paths.Keys);
    }

    private sealed class StubUserRepository(IReadOnlyList<User> users) : IUserRepository
    {
        public Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken) =>
            Task.FromResult(users);

        public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
            Task.FromResult(users.SingleOrDefault(user => user.Id == id));
    }
}
