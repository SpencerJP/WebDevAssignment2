using ASPNETAssignment.Models;

namespace ASPNETAssignment.Models
{
    public class ProcessStockRequestErrorViewModel
    {
        public StockRequest StockRequest { get; set; }

        public ProcessStockRequestErrorViewModel(StockRequest stockRequest)
        {
            StockRequest = stockRequest;
        }
    }
}