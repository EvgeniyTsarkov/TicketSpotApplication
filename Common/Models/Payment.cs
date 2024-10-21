﻿namespace Common.Models;

public class Payment
{
    public int Id { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public decimal TotalAmount { get; set; }
}
