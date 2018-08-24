using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Infrastructure.Enums
{
    /// <summary>
    /// 广告状态枚举
    /// </summary>
    public enum AdsStatusEnum
    {
        /// <summary>
        /// 草稿
        /// </summary>
        Draft = 1,

        /// <summary>
        /// 等待审批
        /// </summary>
        Pending = 2,

        /// <summary>
        /// 准备投放（审批通过）
        /// </summary>
        Approved = 3,

        /// <summary>
        /// 暂停
        /// </summary>
        Pause = 4,

        /// <summary>
        /// 结束
        /// </summary>
        End = 5
    }

    /// <summary>
    /// 广告形式枚举
    /// 创建人：hongbolee
    /// 创建时间：2015-04-02
    /// </summary>
    public enum MaterialsTypeEnum
    {
        /// <summary>
        /// 图片
        /// </summary>
        Image = 1,

        /// <summary>
        /// 图文
        /// </summary>
        ImageText = 2,

        /// <summary>
        /// HTML5
        /// </summary>
        HTML5 = 3,
        /// <summary>
        /// 视频
        /// </summary>
        Video = 4,
        /// <summary>
        /// 原生
        /// </summary>
        Native = 5
    }
    /// <summary>
    /// 广告交互方式
    /// 创建人：hongbolee
    /// 创建时间：2015-04-09
    /// </summary>
    public enum ClickEffectType
    {
        /// <summary>
        /// 打开网页
        /// </summary>
        WebSite = 1,

        /// <summary>
        /// 下载应用
        /// </summary>
        DownloadApp = 2,

        /// <summary>
        /// 打开全屏图片
        /// </summary>
        OpenImage = 3,

        /// <summary>
        /// 拨打电话
        /// </summary>
        CallPhone = 5

    }
    /// <summary>
    /// 创意审核状态
    /// 创建人：hongbolee
    /// 创建时间：2015-04-09
    /// </summary>
    public enum CreativeStatusEnum
    {
        /// <summary>
        /// -2：草稿
        /// </summary>
        Draft = -2,
        /// <summary>
        /// -1：待审核
        /// </summary>
        Pending = -1,
        /// <summary>
        /// 0：审核不通过
        /// </summary>
        UnApproved = 0,
        /// <summary>
        /// 1：审核通过
        /// </summary>
        Approved = 1
    }

    /// <summary>
    /// 广告主状态(0：未激活  1：激活失败 2：已启用  3：已停用 4：已删除
    /// </summary>
    public enum AdvertiserStatus
    {
        /// <summary>
        /// 0：未激活  
        /// </summary>
        UnActivied = 0,
        /// <summary>
        /// 1：激活失败 
        /// </summary>
        ActiviedFail = 1,
        /// <summary>
        /// 2：已启用  
        /// </summary>
        Enable = 2,
        /// <summary>
        /// 3：已停用 
        /// </summary>
        Disabled = 3,
        /// <summary>
        /// 4：已删除
        /// </summary>
        Deleted = 4
    }

    /// <summary>
    /// 计费模式
    /// </summary>
    public enum PayMode
    {
        /// <summary>
        /// CPM	 计费模式
        /// </summary>
        ECPM = 1,

        /// <summary>
        /// CPC	 计费模式
        /// </summary>
        ECPC = 2
    }

    /// <summary>
    /// 投放模式
    /// </summary>
    public enum PutInMode
    {
        /// <summary>
        /// 加速投放
        /// </summary>
        加速投放 = 1,
        /// <summary>
        /// 匀速投放
        /// </summary>
        匀速投放 = 2


    }

    /// <summary>
    /// 竞价算法
    /// </summary>
    public enum Bidding
    {
        /// <summary>
        /// 固定出价
        /// </summary>
        固定出价 = 1,

        /// <summary>
        /// 遗传算法
        /// </summary>
        遗传算法 = 2,

        /// <summary>
        /// 潮汐算法
        /// </summary>
        潮汐算法 = 3
    }

    /// <summary>
    /// 选择创意错误类型
    /// </summary>
    public enum ErrorType
    {
        SessionTimeout,
        InvalidError,
        ExceptionError,
        SameRecords,
        AdScheduleError
    }


    /// <summary>
    /// 广告操作类型
    /// </summary>
    public enum AdOperationType
    {
        /// <summary>
        /// //新建
        /// </summary>
        New = 1,
        /// <summary>
        /// //编辑
        /// </summary>
        Edit = 2,
        /// <summary>
        /// //删除
        /// </summary>
        Delete = 3,
        /// <summary>
        /// //审批
        /// </summary>
        Approve = 4,
        /// <summary>
        /// //关闭/暂停
        /// </summary>
        Stop = 5,
        /// <summary>
        /// //开启/投放
        /// </summary>
        Start = 6,
        /// <summary>
        /// //提交
        /// </summary>
        Submit = 7,
        /// <summary>
        /// //其他
        /// </summary>
        Other = 8
    }

    /// <summary>
    /// 
    /// </summary>
    //public enum AdSaveType
    //{
    //    /// <summary>
    //    /// 修改前
    //    /// </summary>
    //    Before = 0,

    //    /// <summary>
    //    /// 修改后
    //    /// </summary>
    //    After = 1
    //}

    /// <summary>
    /// 广告操作对象
    /// </summary>
    public enum AdOperationObject
    {
        /// <summary>
        ///  广告策略
        /// </summary>
        Ads = 1,
        /// <summary>
        /// 广告排期
        /// </summary>
        //Schedule = 2,
        /// <summary>
        /// 竞价算法
        /// </summary>
        //Adbidding = 3,
        /// <summary>
        /// 定向策略
        /// </summary>
        Orientation = 2,
        /// <summary>
        /// 创意信息
        /// </summary>
        Creative = 3,
        ///// <summary>
        ///// 推广计划
        ///// </summary>
        //AdActivity = 6
        /// <summary>
        /// 其他
        /// </summary>
        Other = 4
    }

    public enum NativeAdType
    {
        /// <summary>
        /// 淘宝综合类
        /// </summary>
        TaoBao = 1,
        /// <summary>
        /// 芒果综合类
        /// </summary>
        Mango = 2,
        /// <summary>
        /// 搜狐新闻端
        /// </summary>
        Sohu = 3
    }

    public enum AdToOptimizeTarget
    {
        /// <summary>
        /// 点击率
        /// </summary>
        ClickRate = 0
    }
    public enum AdToOptimizeIntensity
    {
        /// <summary>
        /// 最低
        /// </summary>
        VeryLow = 0,
        /// <summary>
        /// 低
        /// </summary>
        Low = 1,
        /// <summary>
        /// 中
        /// </summary>
        Middle = 2,
        /// <summary>
        /// 高
        /// </summary>
        Hight = 3,
        /// <summary>
        /// 最高
        /// </summary>
        VeryHight = 4
    }
    //public enum AdToOptimizeHz
    //{
    //    /// <summary>
    //    /// 6小时
    //    /// </summary>
    //    Hz06 = 0,
    //    /// <summary>
    //    /// 12小时
    //    /// </summary>
    //    Hz12 = 1,
    //    /// <summary>
    //    /// 24小时
    //    /// </summary>
    //    Hz24 = 2
    //}
    /// <summary>
    ///访客洞察数据生成状态
    /// </summary>
    public enum RetargetStatus
    {
        /// <summary>
        /// 计算中
        /// </summary>
        Processing = 0,
        /// <summary>
        /// 完成
        /// </summary>
        Completed = 1
    }

    /// <summary>
    /// 获取App方式
    /// </summary>
    public enum GetAppType
    {
        /// <summary>
        /// 推荐
        /// </summary>
        Recommend = 0,
        /// <summary>
        /// top 10
        /// </summary>
        TOP10=1,
        /// <summary>
        /// 后10
        /// </summary>
        LAST10=2
    }

    /// <summary>
    /// 财务明细类型
    /// </summary>
    public enum FinancialType
    {
        /// <summary>
        /// 充值
        /// </summary>
        Recharge = 1,
        /// <summary>
        /// 消费
        /// </summary>
        Payment = 2,
        /// <summary>
        /// 结算
        /// </summary>
        Purchase = 3
    }
}
