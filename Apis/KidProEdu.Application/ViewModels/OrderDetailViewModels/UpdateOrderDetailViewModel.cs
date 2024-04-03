using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.OrderDetailViewModels
{
    public class UpdateOrderDetailViewModel
    {
        public Guid OrderId { get; set; }
        public Guid CourseId { get; set; }
        public int Quantity { get; set; }
        public int PayType { get; set; }
        public int? InstallmentTerm { get; set; }
        public Guid ChildrenProfildId { get; set; }
        public string? EWalletMethod { get; set; }
    }
    public class ReturnOrderDetailViewModel
    {
        public string CourseCode { get; set; }
        public int? Quantity { get; set; }
        public double? UnitPrice { get; set; }
        public int? InstallmentTerm { get; set; }
        public string? PayType { get; set; }
        public string? ChildrenName { get; set; }

    }

    public class PaymentInformationView
    {
        public string CourseCode { get; set; }
        public string Month { get; set; }
        public decimal AmountPerMonth { get; set; }
    }

    public class ReturnPaymentInformationView
    {
        public Guid OrderId { get; set; }
        public List<ReturnOrderDetailViewModel> returnOrderDetailViews { get; set; }
        public List<PaymentInformationView> paymentInformation { get; set; }
        public DateTime CreationDate { get; set; }
        public string? EWalletMethod { get; set; }
        public decimal Total { get;set; }
    }
}
