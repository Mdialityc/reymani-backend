using reymani_web_api.Data;
using FastEndpoints;
using reymani_web_api.Endpoints.Users.Responses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace reymani_web_api.Endpoints.Users
{
  public class GetMeEndpoint : EndpointWithoutRequest<Results<Ok<UserResponse>, NotFound>>
  {
    private readonly AppDbContext _dbContext;

    public GetMeEndpoint(AppDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public override void Configure()
    {
      Get("/users/me");
    }

    public override async Task<Results<Ok<UserResponse>, NotFound>> ExecuteAsync(CancellationToken ct)
    {
      // Extraer el claim "Id" del usuario a partir del JWT
      var userIdClaim = User.Claims.First(c => c.Type == "Id");
      int userId = int.Parse(userIdClaim.Value);

      var user = await _dbContext.Users.FindAsync(new object[] { userId }, ct);
      if (user is null)
        return TypedResults.NotFound();

      return TypedResults.Ok(new UserResponse
      {
        Id = user.Id,
        ProfilePicture = user.ProfilePicture,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
        Phone = user.Phone,
        IsActive = user.IsActive,
        Role = user.Role,
        IsConfirmed = user.IsConfirmed
      });
    }
  }
}
