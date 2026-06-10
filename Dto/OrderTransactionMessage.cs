using System;

namespace ChineseRaffleApi.Dto
{
    public class OrderTransactionMessage
    {
        public string TransactionId { get; set; } = Guid.NewGuid().ToString();
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
