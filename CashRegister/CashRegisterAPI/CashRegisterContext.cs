using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegisterAPI
{
    public class CashRegisterContext : DbContext
    {
        public CashRegisterContext(DbContextOptions<CashRegisterContext> options)
    : base(options)
        { }
        public DbSet<Product> Products { get; set; }
        public DbSet<ReceiptLine> ReceiptLines { get; set; }

        public DbSet<Receipt> Receipts { get; set; }
    }
}
