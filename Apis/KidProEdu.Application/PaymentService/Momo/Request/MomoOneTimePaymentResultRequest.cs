﻿using KidProEdu.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.PaymentService.Momo.Request
{
    public class MomoOneTimePaymentResultRequest
    {
        public string? partnerCode { get; set; } = string.Empty;
        public string? orderId { get; set; } = string.Empty;
        public string? requestId { get; set; } = string.Empty;
        public long amount { get; set; }
        public string? orderInfo { get; set; } = string.Empty;
        public string? orderType { get; set; } = string.Empty;
        public string? transId { get; set; } = string.Empty;
        public string? message { get; set; } = string.Empty;
        public int resultCode { get; set; }
        public string? payType { get; set; } = string.Empty;
        public long responseTime { get; set; }
        public string? extraData { get; set; } = string.Empty;
        public string? signature { get; set; } = string.Empty;

        public bool IsValidSignature(string accessKey, string secretKey)
        {
            var rawHash = "accessKey=" + accessKey +
                   "&amount=" + this.amount +
                   "&extraData=" + this.extraData +
                   "&message=" + this.message +
                   "&orderId=" + this.orderId +
                   "&orderInfo=" + this.orderInfo +
                   "&orderType=" + this.orderType +
                   "&partnerCode=" + this.partnerCode +
                   "&payType=" + this.payType +
                   "&requestId=" + this.requestId +
                   "&responseTime=" + this.responseTime +
                   "&resultCode=" + this.resultCode +
                   "&transId=" + this.transId;
            var checkSignature = StringUtils.HmacSHA256(rawHash, secretKey);
            return this.signature.Equals(checkSignature);
        }
    }
}
