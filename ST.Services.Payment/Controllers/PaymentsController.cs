using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SC.Internship.Common.ScResult;
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
    public async Task<ScResult<Guid>> CreatePayment(CancellationToken cancellationToken)
    {
        var transactionId = _paymentsInstance.AddTransactionToPool(cancellationToken);

        return await Task.FromResult(new ScResult<Guid>(transactionId));
    }

    [HttpPut("{transactionId:guid}/{state}")]
    public async Task<ScResult> ChangePaymentState([FromRoute] Guid transactionId, [FromRoute] string state, CancellationToken cancellationToken)
    {
        Enum.TryParse(state, out PaymentState paymentState);
        _paymentsInstance.ChangePaymentState(paymentState, transactionId, cancellationToken);

        return await Task.FromResult(new ScResult());
    }
}