using SC.Internship.Common.Exceptions;
using ST.Services.Payment.Enumerics;
using ST.Services.Payment.Interfaces;

namespace ST.Services.Payment.Models;

public class PaymentsSingleton : IPaymentSingleton
{
    public List<PaymentsTransaction> PaymentTransactions { get; set; } = new();

    public void SetTransactionAsCanceled(Guid transactionId, CancellationToken cancellationToken)
    {
        var transaction = PaymentTransactions.FirstOrDefault(p => p.Id == transactionId);

        if (transaction == null) return;
        
        transaction.DateCancellation = DateTimeOffset.UtcNow;
        transaction.State = PaymentState.Canceled;
    }

    public void SetTransactionAsConfirm(Guid transactionId, CancellationToken cancellationToken)
    {
        var transaction = PaymentTransactions.FirstOrDefault(p => p.Id == transactionId);

        if (transaction == null) return;

        transaction.DateConfirmation = DateTimeOffset.UtcNow;
        transaction.State = PaymentState.Confirmed;
    }

    public Guid AddTransactionToPool(CancellationToken cancellationToken)
    {
        var paymentTransaction = new PaymentsTransaction();
        PaymentTransactions.Add(paymentTransaction);

        return paymentTransaction.Id;
    }

    public void ChangePaymentState(PaymentState state, Guid transactionId, CancellationToken cancellationToken)
    {
        switch (state)
        {
            case PaymentState.Canceled:
                SetTransactionAsCanceled(transactionId, cancellationToken);
                break;
            case PaymentState.Hold:
                break;
            case PaymentState.Confirmed:
                SetTransactionAsConfirm(transactionId, cancellationToken);
                break;
            default:
                throw new ScException("Не найден текущий статус транзакции");
        }
    }
}