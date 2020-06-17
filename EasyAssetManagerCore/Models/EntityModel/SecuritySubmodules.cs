using EasyAssetManagerCore.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAssetManagerCore.Models.EntityModel
{
    [TableAttribute("submodules", "security")]
    public partial class SecuritySubmodules
    {
        public SecuritySubmodules()
        {
           // this.Sections = new HashSet<SecuritySubmodulesections>();
        }
        public string SubModuleId { get; set; }
        public string ModuleId { get; set; }
        public string SubModuleName { get; set; }
        public string AreaName { get; set; }
        public string NextScreenCode { get; set; }
        public DateTime? SetDate { get; set; }
        public string UserId { get; set; }
        public string IconName { get; set; }

       // public IEnumerable<SecuritySubmodulesections> Sections { get; set; }

    }
}
