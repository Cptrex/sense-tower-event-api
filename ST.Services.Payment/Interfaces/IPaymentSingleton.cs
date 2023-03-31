using ST.Services.Payment.Enumerics;
using ST.Services.Payment.Models;
// ReSharper disable UnusedMemberInSuper.Global

namespace ST.Services.Payment.Interfaces;

public interface IPaymentSingleton
{
    public List<PaymentsTransaction> PaymentTransactions { get; set; }
    public void SetTransactionAsCanceled(Guid transactionId, CancellationToken cancellationToken);
    public void SetTransactionAsConfirm(Guid transactionId, CancellationToken cancellationToken);
    public Guid AddTransactionToPool(CancellationToken cancellationToken);
    public void ChangePaymentState(PaymentState state, Guid transactionId, CancellationToken cancellationToken);
}