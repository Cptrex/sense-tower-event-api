using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SC.Internship.Common.ScResult;

namespace ST.Services.Image.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ImagesController : ControllerBase
{
    [HttpGet("{imageId:guid}")]
    public ScResult<bool> IsImageExist([FromRoute] Guid imageId, CancellationToken cancellationToken)
    {
        return new ScResult<bool>(true);
    }
}