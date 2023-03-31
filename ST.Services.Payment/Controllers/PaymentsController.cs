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
    private readonly IPaymentSingleton _paymentInstance;

    public PaymentsController(IPaymentSingleton paymentInstance)
    {
        _paymentInstance = paymentInstance;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment(CancellationToken cancellationToken)
    {
        var transactionId = _paymentInstance.AddTransactionToPool(cancellationToken);

        return await Task.FromResult(Ok(transactionId));
    }

    [HttpPut("/{transactionId:guid}/state={state:int}")]
    public async Task<IActionResult> ChangePaymentState([FromRoute] Guid transactionId, [FromRoute] int state, CancellationToken cancellationToken)
    {
        _paymentInstance.ChangePaymentState((PaymentState) state, transactionId, cancellationToken);

        return await Task.FromResult(Ok());
    }
}