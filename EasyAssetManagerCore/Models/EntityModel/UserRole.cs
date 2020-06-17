using System.Collections.Generic;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class UserRole
    {
        public string rolE_ID { get; set; }
        public string ROLE_DESC { get; set; }
        public string ROLE_Name { get; set; }
        public string role_slno { get; set; }        
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string user_desc { get; set; }
        public string action { get; set; }
        public string ins_by { get; set; }
        public string ins_date { get; set; }
        public string status { get; set; }
        public IEnumerable<ScreenAccessPermission> ScreenAccessPermissions { get; set; }
    }
}
