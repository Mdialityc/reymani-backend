using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.CostoEnvio;

public class GetAllCostoEnvioByIdNegocioResponse
{
  public List<CostoEnvioDto> CostosEnvios { get; set; } = new List<CostoEnvioDto>();
}