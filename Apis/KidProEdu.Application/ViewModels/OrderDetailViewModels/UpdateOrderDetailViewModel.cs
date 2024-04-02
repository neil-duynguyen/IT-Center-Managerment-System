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
        public int PayType { get; set; }
        public int? InstallmentTerm { get; set; }
        public Guid ChildrenProfildId { get; set; }
        public string? EWalletMethod { get; set; }
    }

    public class PaymentInformationView
    {
        public string CourseCode { get; set; }
        public string Month { get; set; }
        public decimal AmountPerMonth { get; set; }
    }

    public class ReturnPaymentInformationView
    {
        public List<UpdateOrderDetailViewModel> orderDetailViewModels { get; set; }
        public List<PaymentInformationView> paymentInformation { get; set; }

    }
}
