using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SC.Internship.Common.ScResult;

namespace ST.Services.Image.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ImagesController : ControllerBase
{
    [HttpGet("/{imageId:guid}")]
    public async Task<ScResult<bool>> IsImageExist([FromRoute] Guid imageId, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new ScResult<bool>(true));
    }
}