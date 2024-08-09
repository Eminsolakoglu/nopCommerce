using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Shipping.RoutePlanner.Models
{
    public class ConfigurationModel:BaseEntity
    {        //projede birden fazla mağaza varsa aktif mağazaya göre configurasyon yapmasını yarıyor

        public int ActiveStoreScopeConfiguration { get; set; }
        [NopResourceDisplayName("Plugin.Shipping.RoutePlanner.Configuration.WidgetzoneContent")]
        //modelde alttaki özellik geldiğinde üstü okuyacak içine ne atadıysak içinde hinti okuyacak modelde
        public string WidgetzoneContent { get; set; }//konfigurasyon sayfasının modeli
        public bool WidgetzoneContent_OverrideForStore { get; set; }//mağaza için geçerli mi değil mi onu kontrol ediyor

        public int OrderCount { get; set; }
    }
}
