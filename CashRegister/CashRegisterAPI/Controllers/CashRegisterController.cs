using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CashRegisterAPI.Controllers
{
    [ApiController]
    public class CashRegisterController : ControllerBase
    {
        private readonly IDataAccess dal;

        private readonly ILogger<CashRegisterController> _logger;

        public CashRegisterController(IDataAccess _dal, ILogger<CashRegisterController> logger)
        {
            _logger = logger;
            this.dal = _dal;
        }

        [HttpGet]
        [Route("products")]
        public IEnumerable<Product> GetProducts()
        {
            return dal.GetProducts();
        }
        [HttpGet]
        [Route("receiptlines")]
        public IEnumerable<ReceiptLine> GetReceiptLines()
        {
            return dal.GetReceiptLines();
        }
        [HttpPost]
        [Route("receiptlines/add")]
        public Task<List<ReceiptLine>> AddReceiptLine([FromBody] string name)
        {
            return dal.AddReceiptLine(name);
        }
        [HttpGet]
        [Route("checkout")]
        public async Task<Receipt> Checkout()
        {
            return await dal.Checkout();
        }

    }
}
