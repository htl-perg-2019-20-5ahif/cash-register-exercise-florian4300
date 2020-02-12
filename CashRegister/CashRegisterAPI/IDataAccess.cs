using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegisterAPI
{
    public interface IDataAccess : IDisposable
    {
        public IEnumerable<Product> GetProducts();
        public Task<List<ReceiptLine>> AddReceiptLine(string name);

        public IEnumerable<ReceiptLine> GetReceiptLines();

        public decimal calculateTotalPriceOfReceipts();

        public Task<Receipt> Checkout();
    }
}
