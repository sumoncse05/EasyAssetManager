using EasyAssetManagerCore.Model.CommonModel;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class Customer
    {
        public Customer()
        {
            Message = new Message();
            CustomerImage = new CustomerImage();
        }
       public string customer_status { get; set; }
        public string mobile_number { get; set; }
        public string date_of_birth { get; set; }
        public string customer_no { get; set; }
        public string customer_name { get; set; }
        public string father_name { get; set; }
        public string mother_name { get; set; }
        public string sex { get; set; }
        public string nid { get; set; }
        public string customer_account_no { get; set; }
        public Message Message { get; set; }

        public CustomerImage CustomerImage { get; set; }
    }
}
