using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ST.Services.Payment.Interfaces;

namespace ST.Services.Payment.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentSingleton _paymentInstance;

    public PaymentController(IPaymentSingleton paymentInstance)
    {
        _paymentInstance = paymentInstance;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment(CancellationToken cancellationToken)
    {
        var transactionId = _paymentInstance.AddTransactionToPool(cancellationToken);

        return await Task.FromResult(Ok(transactionId));
    }

    [HttpPut]
    public async Task<IActionResult> ConfirmPayment(Guid createdTransactionId, CancellationToken cancellationToken)
    {
        _paymentInstance.SetTransactionAsConfirm(createdTransactionId, cancellationToken);

        return await Task.FromResult(Ok());
    }

    [HttpPatch]
    public async Task<IActionResult> CancelPayment(Guid createdTransactionId, CancellationToken cancellationToken)
    {
        _paymentInstance.SetTransactionAsCanceled(createdTransactionId, cancellationToken);

        return await Task.FromResult(Ok());
    }
}