using System;

namespace EasyAssetManagerCore.Model.CommonModel
{
    public class BaseModel
    {
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
        public DateTime ModifyDate { get; set; }
        public int LastModUserId { get; set; }
    }
}
