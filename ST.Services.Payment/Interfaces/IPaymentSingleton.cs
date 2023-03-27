using JetBrains.Annotations;
using ST.Services.Payment.Models;

namespace ST.Services.Payment.Interfaces;

public interface IPaymentSingleton
{
    [UsedImplicitly] public List<PaymentTransaction> PaymentTransactions { get; set; }

    public void SetTransactionAsCanceled(Guid transactionId, CancellationToken cancellationToken);
    public void SetTransactionAsConfirm(Guid transactionId, CancellationToken cancellationToken);
    public Guid AddTransactionToPool(CancellationToken cancellationToken);
}