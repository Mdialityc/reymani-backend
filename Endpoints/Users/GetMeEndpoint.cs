using reymani_web_api.Data;
using FastEndpoints;
using reymani_web_api.Endpoints.Users.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using reymani_web_api.Services.BlobServices;
using reymani_web_api.Endpoints.Mappers;

namespace reymani_web_api.Endpoints.Users
{
  public class GetMeEndpoint : EndpointWithoutRequest<Results<Ok<UserResponse>, NotFound>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public GetMeEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Get("/users/me");
      Summary(s =>
      {
        s.Summary = "Get current user";
        s.Description = "Retrieves details of the authenticated user.";
      });
    }

    public override async Task<Results<Ok<UserResponse>, NotFound>> ExecuteAsync(CancellationToken ct)
    {
      // Extraer el claim "Id" del usuario a partir del JWT
      var userIdClaim = User.Claims.First(c => c.Type == "Id");
      int userId = int.Parse(userIdClaim.Value);

      var user = await _dbContext.Users.FindAsync(new object[] { userId }, ct);
      if (user is null)
        return TypedResults.NotFound();

      var mapper = new UserMapper();
      var response = mapper.FromEntity(user);
      if (!string.IsNullOrEmpty(user.ProfilePicture))
        response.ProfilePicture = await _blobService.PresignedGetUrl(user.ProfilePicture, ct);

      return TypedResults.Ok(response);
    }
  }
}
