using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SC.Internship.Common.ScResult;

namespace SenseTowerEventAPI;

[ApiController]
[Route("[controller]")]
// ReSharper disable once InconsistentNaming. Решарпер показывает излишнюю рекомендацию
public class StubController : ControllerBase
{
    /// <summary>
    /// Проверка JWT аутентификации
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("authstub")]
    public async Task<ScResult> AuthStub()
    {
        return await Task.FromResult(new ScResult());
    }
}