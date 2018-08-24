using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Infrastructure.Enums
{
    /// <summary>
    /// 访客来源枚举 
    /// </summary>
    public enum VisitGroupEnum
    {
        /// <summary>
        /// 策略访客
        /// </summary>
        AdsVisit = 1,

        /// <summary>
        /// 导入访客
        /// </summary>
        ImportVisit
    }


    public enum VisitType
    {
        /// <summary>
        /// 曝光访客
        /// </summary>
        ShowVisit = 1,

        /// <summary>
        /// 曝光点击访客
        /// </summary>
        ClickVisit = 2,

        /// <summary>
        /// 曝光未点击访客(潜在访客)
        /// </summary>
        ShowNotClick = 3
    }


}
