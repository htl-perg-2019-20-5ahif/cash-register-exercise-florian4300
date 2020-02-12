using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegisterAPI
{
    public class DataAccess : IDataAccess
    {

        public CashRegisterContext context;

        public DataAccess(CashRegisterContext _context)
        {
            context = _context;
        }
        public async Task<List<ReceiptLine>> AddReceiptLine(string name)
        {
            if (this.context.ReceiptLines.Include(rl => rl.BoughtProduct).Any(rl => rl.BoughtProduct.ProductName.Equals(name)))
            {
                ReceiptLine temp = this.context.ReceiptLines.Include(rl => rl.BoughtProduct).Where(rl => rl.BoughtProduct.ProductName.Equals(name)).FirstOrDefault();
                temp.Amount += 1;
                temp.TotalPrice = decimal.Multiply(temp.BoughtProduct.UnitPrice, temp.Amount);
                await this.context.SaveChangesAsync();
            }
            else
            {
                Product p = this.context.Products.Where(p => p.ProductName == name).FirstOrDefault();
                ReceiptLine rl = new ReceiptLine();
                rl.Amount = 1;
                rl.BoughtProduct = p;
                rl.BoughtProductId = p.ProductId;
                rl.TotalPrice = p.UnitPrice;
                this.context.ReceiptLines.Add(rl);
                await this.context.SaveChangesAsync();
            }
            return this.context.ReceiptLines.Include(rl => rl.BoughtProduct).ToList();
        }

        public decimal calculateTotalPriceOfReceipts()
        {
            decimal sum = 0;
            foreach(ReceiptLine rl in this.context.ReceiptLines.ToList())
            {
                sum += rl.TotalPrice;
            }
            return sum;
        }

        public void Dispose()
        {
            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        public IEnumerable<Product> GetProducts()
        {
            return this.context.Products.ToList();
        }

        public IEnumerable<ReceiptLine> GetReceiptLines()
        {
            return this.context.ReceiptLines.ToList();
        }

        public async Task<Receipt> Checkout()
        {
            Receipt rc = new Receipt();
            rc.ReceiptTimestamp = DateTime.Now;
            rc.TotalPrice = this.calculateTotalPriceOfReceipts();
            rc.ReceiptLines = this.context.ReceiptLines.ToList();
            this.context.Receipts.Add(rc);
            this.context.ReceiptLines.RemoveRange(this.context.ReceiptLines.ToList());
            await this.context.SaveChangesAsync();
            return rc;
        }
    }
}
