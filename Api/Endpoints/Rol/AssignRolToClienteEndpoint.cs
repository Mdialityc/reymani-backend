
namespace reymani_web_api.Api.Endpoints.Rol;

public class AssignRolToClienteEndpoint : Endpoint<AssingRoleToClienteRequest>
{
  private readonly IClienteService _clienteService;

  public AssignRolToClienteEndpoint(IClienteService clienteService)
  {
    _clienteService = clienteService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/roles/assign_role_to_client");
    Summary(s =>
    {
      s.Summary = "Asignar Rol a Cliente";
      s.Description = "Asigna un rol a un cliente";
      s.ExampleRequest = new AssingRoleToClienteRequest
      {
        ClienteId = Guid.NewGuid(),
        RoleId = Guid.NewGuid()
      };
    });
  }

  public override async Task HandleAsync(AssingRoleToClienteRequest req, CancellationToken ct)
  {
    await _clienteService.AssignRoleToClienteAsync(req.ClienteId, req.RoleId);
    await SendOkAsync(ct);
  }
}
