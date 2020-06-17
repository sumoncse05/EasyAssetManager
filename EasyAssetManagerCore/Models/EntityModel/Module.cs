using EasyAssetManagerCore.Model.CommonModel;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class Module
    {
        public Module()
        {
            Message = new Message();
        }
        public string mod_id { get; set; }
        public string mod_desc { get; set; }
        public Message Message { get; set; }
    }
}
