using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SC.Internship.Common.ScResult;

namespace ST.Events.API;

[ApiController]
[Route("[controller]")]
[Authorize]
// ReSharper disable once InconsistentNaming. Решарпер показывает излишнюю рекомендацию
public class StubController : ControllerBase
{
    /// <summary>
    /// Проверка JWT аутентификации
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("authstub")]
    public async Task<ScResult<IActionResult>> AuthStub()
    {
        return await Task.FromResult(new ScResult<IActionResult>(Ok("Ok")));
    }
}