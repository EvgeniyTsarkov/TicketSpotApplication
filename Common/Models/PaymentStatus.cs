namespace Common.Models;

public enum PaymentStatus
{
    Pending, 
    Completed, 
    Failed,
    Cancelled, 
    Refunded,
    InProgress, 
    Disputed, 
    Expired
}
