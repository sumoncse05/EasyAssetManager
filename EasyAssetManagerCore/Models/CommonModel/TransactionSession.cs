using EasyAssetManagerCore.Models.EntityModel;
using System.Collections.Generic;

namespace EasyAssetManagerCore.Models.CommonModel
{
    public class TransactionSession
    {
        public string TransactionCustomerNo { get; set; }
        public string TransactionCustMobileNo { get; set; }
        public string TransactionCustPhotoType { get; set; }
        public string TransactionType { get; set; }
        public string TransactionDate { get; set; }
        public string SmsReqRefNo { get; set; }
        public string SmsOtpData { get; set; }
        public string SmsOtpVerifyStatus { get; set; }
        public string FingerVerifyStatus { get; set; }
        public string TransactionID { get; set; }
        public string FingerReqRefNo { get; set; }
        public string UbsTransactionRefNo { get; set; }
        public string TransactionAccountNo { get; set; }
        public string TransactionAmount { get; set; }
        public string RemittanceAccount { get; set; }
        public string TransactionAccountDesc { get; set; }
        public string TransactionCharge { get; set; }
        public string TransactionChargeVat { get; set; }
        public string TransactionTotalAmount { get; set; }
        public int CustomerValidated { get; set; }
        public int AccountOperatingMode { get; set; }
        public string AlternetAccountNo { get; set; }
        public string AlternetAccountDesc { get; set; }
        public string BillRef { get; set; }
        public string TransactionAmountVat { get; set; }
        public string CustomerName { get; set; }
        public string CustomerFatherName { get; set; }
        public string CustomerMotherName { get; set; }
        public string CustomerDateofBirth { get; set; }
        public string CustomerSex { get; set; }
        public string CustomerNID { get; set; }
        public string TransactionVatAmount { get; set; }
        public string BearerType { get; set; }
        public string BearerTypeDesc { get; set; }
    }
}
