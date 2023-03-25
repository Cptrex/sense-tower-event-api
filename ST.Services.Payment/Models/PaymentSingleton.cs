using ST.Services.Payment.Interfaces;
using ST.Services.Payment.Utils;

namespace ST.Services.Payment.Models;

public class PaymentSingleton : IPaymentSingleton
{
    public List<PaymentTransaction> PaymentTransactions { get; set; } = new();

    public void SetTransactionAsCanceled(Guid transactionId)
    {
        var transaction = PaymentTransactions.FirstOrDefault(p => p.Id == transactionId);

        if (transaction == null) return;
        
        transaction.DateCancellation = DateTime.UtcNow;
        transaction.State = PaymentState.Canceled;
    }

    public void SetTransactionAsConfirm(Guid transactionId)
    {
        var transaction = PaymentTransactions.FirstOrDefault(p => p.Id == transactionId);

        if (transaction != null) return;
        transaction.DateConfirmation = DateTime.UtcNow;
        transaction.State = PaymentState.Confirmed;
    }

    public Guid AddTransactionToPool()
    {
        var paymentTransaction = new PaymentTransaction();
        PaymentTransactions.Add(paymentTransaction);

        return paymentTransaction.Id;
    }
}