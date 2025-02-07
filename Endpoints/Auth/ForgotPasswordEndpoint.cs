using System;

using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Data.Models;
using reymani_web_api.Endpoints.Auth.Requests;
using reymani_web_api.Services.EmailServices;

namespace reymani_web_api.Endpoints.Auth
{
  public class ForgotPasswordEndpoint : Endpoint<ForgotPasswordRequest, Results<Ok<string>, NotFound<string>>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IEmailSender _emailSender;

    public ForgotPasswordEndpoint(AppDbContext dbContext, IEmailSender emailSender)
    {
      _dbContext = dbContext;
      _emailSender = emailSender;
    }

    public override void Configure()
    {
      Post("/auth/forgot-password");
      AllowAnonymous();
      Summary(s =>
      {
        s.Summary = "Forgot Password";
        s.Description = "Generate a password reset code and send it via email";
      });
    }

    public override async Task<Results<Ok<string>, NotFound<string>>> ExecuteAsync(ForgotPasswordRequest request, CancellationToken ct)
    {
      var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == request.Email, ct);
      if (user == null)
        return TypedResults.Ok("A reset code has been sent to your email.");

      // Generar código de restablecimiento de 4 dígitos
      var rnd = new Random();
      int resetCode = rnd.Next(1000, 10000);

      // Almacenar el código de reseteo en la BD
      var passwordReset = new ForgotPasswordNumber
      {
        UserId = user.Id,
        Number = resetCode.ToString()
      };
      _dbContext.Set<ForgotPasswordNumber>().Add(passwordReset);
      await _dbContext.SaveChangesAsync(ct);

      // Enviar correo con el código de restablecimiento
      await _emailSender.SendEmailAsync(user.Email, "Reset your password", $"Your password reset code is: {resetCode}");

      return TypedResults.Ok("A reset code has been sent to your email.");
    }
  }
}
