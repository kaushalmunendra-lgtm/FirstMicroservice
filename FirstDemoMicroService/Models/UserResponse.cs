namespace FirstDemoMicroService.Models;

public sealed record UserResponse(int Id, string FullName, string Email, DateTime CreatedAtUtc);
