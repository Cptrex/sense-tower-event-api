using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SC.Internship.Common.ScResult;

namespace ST.Services.Space.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class SpacesController : ControllerBase
{
    [HttpGet("/{spaceId:guid}")]
    public async Task<ScResult<bool>> IsSpaceExist([FromRoute] Guid spaceId, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new ScResult<bool>(true));
    }
}