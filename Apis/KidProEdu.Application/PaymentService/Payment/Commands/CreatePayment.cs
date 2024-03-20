using KidProEdu.Application.Interfaces;
using KidProEdu.Application.PaymentService.Momo.Request;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.PaymentService.Payment.Commands
{
    public class CreatePayment
    {
        public string PaymentContent { get; set; } = string.Empty;
        public string PaymentCurrency { get; set; } = "VND";
        //public string PaymentRefId { get; set; } = string.Empty;
        public decimal? RequiredAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? PaymentLanguage { get; set; } = "vn";
        //public string? MerchantId { get; set; } = string.Empty;
        public string? PaymentDestinationId { get; set; } = "MOMO";
        public string? Signature { get; set; } = string.Empty;
    }

    public class CreatePaymentHandler
    {
        private readonly IConfiguration _configuration;
        private readonly ICurrentTime _currentTime;

        public CreatePaymentHandler(IConfiguration configuration, ICurrentTime currentTime)
        {
            _configuration = configuration;
            _currentTime = currentTime;
        }

        public async Task<string> Handle(string orderId)
        {
            string result;
            try
            {
                var paymentUrl = string.Empty;


                CreatePayment createPayment = new CreatePayment();
                createPayment.PaymentDate = _currentTime.GetCurrentTime();
                createPayment.ExpireDate = _currentTime.GetCurrentTime().AddMinutes(5);
                createPayment.PaymentContent = "Thanh toán";

                /*switch (createPayment.PaymentDestinationId)
                {
                    *//*case "VNPAY":
                        var vnpayPayRequest = new VnpayPayRequest(vnpayConfig.Version,
                            vnpayConfig.TmnCode, DateTime.Now, currentUserService.IpAddress ?? string.Empty, request.RequiredAmount ?? 0, request.PaymentCurrency ?? string.Empty,
                            "other", request.PaymentContent ?? string.Empty, vnpayConfig.ReturnUrl, outputIdParam!.Value?.ToString() ?? string.Empty);
                        paymentUrl = vnpayPayRequest.GetLink(vnpayConfig.PaymentUrl, vnpayConfig.HashSecret);
                        break;*//*
                    case "MOMO":
                        var momoOneTimePayRequest = new MomoOneTimePaymentRequest(_configuration["Momo:PartnerCode"],
                            Guid.NewGuid().ToString(), (long)createPayment.RequiredAmount!, orderId,
                            createPayment.PaymentContent ?? string.Empty, _configuration["Momo:ReturnUrl"], _configuration["Momo:IpnUrl"], "captureWallet",
                            string.Empty);
                        momoOneTimePayRequest.MakeSignature(_configuration["Momo:AccessKey"], _configuration["Momo:SecretKey"]);
                        (bool createMomoLinkResult, string? createMessage) = momoOneTimePayRequest.GetLink(_configuration["Momo:PaymentUrl"]);
                        if (createMomoLinkResult)
                        {
                            paymentUrl = createMessage;
                        }
                        else
                        {
                            result = createMessage;
                        }
                        break;
                    default:
                        break;
                }*/

                result = paymentUrl;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

    }
}
