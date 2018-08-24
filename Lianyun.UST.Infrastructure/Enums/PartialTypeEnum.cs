using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Infrastructure.Enums
{
    /// <summary>
    /// 部分页显示类型枚举
    /// 尹洲
    /// 20150409
    /// </summary>
    public enum PartialTypeEnum
    {
        /// <summary>
        /// 展示量部分页面
        /// </summary>
        CommonPartial = 0,

        /// <summary>
        /// 展示量部分页面--按日展示
        /// </summary>
        DayPartial = 1,

        /// <summary>
        /// 展示量部分页面--按小时展示
        /// </summary>
        HourPartial = 2,

        /// <summary>
        /// 展示量部分页面--只显示受众
        /// </summary>
        PeoplePartial = 3
    }

    /// <summary>
    /// 创意报表对比维度
    /// </summary>
    public enum CreativeDimensionsEnum
    {
        /// <summary>
        /// 展示量
        /// </summary>
        ShowNum = 0,
        /// <summary>
        /// 点击量
        /// </summary>
        ClickNum = 1,
        /// <summary>
        /// 消费
        /// </summary>
        Payment = 2,
        /// <summary>
        /// eCPC
        /// </summary>
        AvgClickPrice,
        /// <summary>
        /// eCPM
        /// </summary>
        ThousandTimesShowPrice,
        /// <summary>
        /// 点击率
        /// </summary>
        ClickNumProbability,

        /// <summary>
        /// 参与竞价数
        /// </summary>
        AuctionNum,
        /// <summary>
        /// 竞价成功数
        /// </summary>
        AuctionSuccessNum,
        /// <summary>
        /// 竞价成功率
        /// </summary>
        AuctionSuccessProbability


    }
}
