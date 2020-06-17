using EasyAssetManagerCore.Model.CommonModel;
using System.Collections.Generic;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class WithdrawCash
    {
        public string transactionAccountNo { get; set; }
        public string transactionAmount { get; set; }
        public string transactionAccountDesc { get; set; }
        public string transactionCharge { get; set; }
        public string transactionChargeVat { get; set; }
        public string transactionTotalAmount { get; set; }
        public string bearerType { get; set; }
        public string bearerTypeDesc { get; set; }
        public string customer_no { get; set; }
        public int accountOperatingMode { get; set; }
        public int customerValidated { get; set; }
        public List<Customer> Customers { get; set; }
        public Message Message { get; set; }
    }
}
