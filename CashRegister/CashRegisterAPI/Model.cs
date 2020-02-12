using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CashRegisterAPI
{
    public class Product
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [Required]
        [JsonPropertyName("productName")]
        public String ProductName { get; set; }

        [Required]
        [JsonPropertyName("unitPrice")]
        public decimal UnitPrice { get; set; }


    }
    public class ReceiptLine
    {
        [JsonPropertyName("receiptLineId")]
        public int ReceiptLineId { get; set; }

        [JsonPropertyName("boughtProduct")]

        public Product BoughtProduct { get; set; }

        [JsonPropertyName("boughtProductId")]

        public int BoughtProductId { get; set; }

        [JsonPropertyName("amount")]

        public int Amount { get; set; }
        [JsonPropertyName("totalPrice")]

        public decimal TotalPrice { get; set; }

    }

    public class Receipt
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("receiptLines")]
        public List<ReceiptLine> ReceiptLines { get; set; }

        [JsonPropertyName("receiptTimestamp")]
        public DateTime ReceiptTimestamp { get; set; }

        [JsonPropertyName("totalPrice")]
        public decimal TotalPrice { get; set; }
    }
}
