using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SC.Internship.Common.ScResult;
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
    public async Task<ScResult<Guid>> CreatePayment()
    {
        var transactionId = _paymentInstance.AddTransactionToPool();

        return await Task.FromResult(new ScResult<Guid>(transactionId));
    }

    [HttpPut]
    public async Task<ScResult> ConfirmPayment(Guid createdTransactionId)
    {
        _paymentInstance.SetTransactionAsConfirm(createdTransactionId);

        return await Task.FromResult(new ScResult());
    }

    [HttpPatch]
    public async Task<ScResult> CancelPayment(Guid createdTransactionId)
    {
        _paymentInstance.SetTransactionAsCanceled(createdTransactionId);

        return await Task.FromResult(new ScResult());
    }
}