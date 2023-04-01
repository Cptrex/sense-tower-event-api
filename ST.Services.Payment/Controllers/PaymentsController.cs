using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ST.Services.Payment.Enumerics;
using ST.Services.Payment.Interfaces;

namespace ST.Services.Payment.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentsSingleton _paymentsInstance;

    public PaymentsController(IPaymentsSingleton paymentsInstance)
    {
        _paymentsInstance = paymentsInstance;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment(CancellationToken cancellationToken)
    {
        var transactionId = _paymentsInstance.AddTransactionToPool(cancellationToken);

        return await Task.FromResult(Ok(transactionId));
    }

    [HttpPut("/{transactionId:guid}/{state:int}")]
    public async Task<IActionResult> ChangePaymentState([FromRoute] Guid transactionId, [FromRoute] int state, CancellationToken cancellationToken)
    {
        _paymentsInstance.ChangePaymentState((PaymentState) state, transactionId, cancellationToken);
        return await Task.FromResult(Ok());
    }
}