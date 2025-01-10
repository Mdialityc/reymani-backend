using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.CategoriaNegocio;

public class GetAllCategoriaNegocioByIdNegocioResponse
{
  public List<CategoriaNegocioDto> CategoriasNegocios { get; set; } = new List<CategoriaNegocioDto>();
}
