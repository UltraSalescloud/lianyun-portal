using Lianyun.UST.Infrastructure.Enums;
using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Model.Lianyun_UST_Entities;
using Lianyun.UST.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Repository
{
    /// <summary>
    /// 广告日志仓储最终实现层
    /// </summary>
    public class AdOperationLogRepository : BaseRepository<AdOperationLog>, IAdOperationLogRepository
    {
        public AdOperationLogRepository(Lianyun_DSPContext lianyun_DSPContext,ILogger logger) : base(lianyun_DSPContext,logger) { }

        /// <summary>
        /// 广告日志主表信息入库
        /// 创建人：朱斌
        /// 创建时间：2015-04-27
        /// </summary>
        /// <param name="code">广告Code</param>
        /// <param name="opType">操作类型（新增，修改，删除...）</param>
        /// <param name="opObject">操作对象（广告活动，广告策略，排期...）</param>
        /// <param name="opContent">操作内容（具体干了什么事情）</param>
        /// <param name="opCode">操作序列</param>
        /// <param name="userCode">操作人Code</param>
        /// <param name="userName">操作人</param>
        /// <param name="adID">广告ID</param>
        public void SaveLog(string code, int opType, int opObject, string opContent, string opCode, string userCode, string userName, long adID = 0)
        {
            string sql = string.Empty;
            sql = @"INSERT INTO AdOperationLog
                       ([AdID]
                       ,[AdCode]
                       ,[OPType]
                       ,[OPObject]
                       ,[OPContent]
                       ,[CreatedOn]
                       ,[CreatedBy]
                       ,[CreatedByName]
                       ,[OPSequence])
                       VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')";
            sql = string.Format(sql, adID, code, opType, opObject, opContent, DateTime.Now, userCode, userName, opCode);
            DB.Database.ExecuteSqlCommand(sql, "");
        }

        /// <summary>
        /// 广告日志详细操作记录入库
        /// 创建人：朱斌
        /// 创建时间：2015-04-27
        /// </summary>
        /// <param name="entity">操作的实体</param>
        /// <param name="opTabName">操作的表名</param>
        /// <param name="opCode">操作序列</param>
        public void SaveObject(object entity, string opTabName,int opType, string opCode)
        {
            if (entity == null) return;

            string strSQL = string.Empty;
            
            System.Data.SqlClient.SqlParameter sqlParam = null;

            switch (opTabName.ToLower())
            {
                /*
            case "dsp_adactivities"://广告活动
                DSP_AdActivities active = entity as DSP_AdActivities;
                if (opType != (int)AdOperationType.New)
                {
                    strSQL += string.Format(@"INSERT INTO AdOperationLog_DSP_AdActivities
                           ([OPSequence]
                           ,[Type]
                           ,[AdvertisersCode]
                           ,[Code]
                           ,[Name]
                           ,[TotalMoney]
                           ,[PlatformType]
                           ,[MobileType])
                           select
                            '{0}'
                           ,'{1}' 
                           ,[AdvertisersCode]
                           ,[Code]
                           ,[Name]
                           ,[TotalMoney]
                           ,[PlatformType]
                           ,[MobileType]
                           from DSP_AdActivities where Code='{2}';", opCode, 0, active.Code);
                }

                if (opType != (int)AdOperationType.Delete)
                {
                    strSQL += string.Format(@"INSERT INTO AdOperationLog_DSP_AdActivities
                           ([OPSequence]
                           ,[Type]
                           ,[AdvertisersCode]
                           ,[Code]
                           ,[Name]
                           ,[TotalMoney]
                           ,[PlatformType]
                           ,[MobileType])
                           select 
                           '{0}'
                           ,'{1}'
                           ,'{2}'
                           ,'{3}'
                           ,'{4}'
                           ,'{5}'
                           ,'{6}'
                           ,'{7}'
                           ,'{8}'", opCode, 1, active.AdvertisersCode, active.Code, active.Name, active.TotalMoney, active.PlatformType, active.MobileType);
                }
                break;*/
                case "dsp_ads"://广告策略
                    DSP_Ads ads = entity as DSP_Ads;
                    if (opType != (int)AdOperationType.New)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_Ads
                                   ([OPSequence]
                                   ,[Type]
                                   ,[Code]
                                   ,[AdvertisersCode]
                                   ,[ActivitiesCode]
                                   ,[Name]
                                   ,[AdMoney]
                                   ,[Remark]
                                   ,[PutInMode]
                                   ,[AdGoalItems]
                                   ,[Status]
                                   ,[Step]
                                   ,[CheckBy]
                                   ,[CheckOn]
                                   ,[AdStartDate]
                                   ,[AdScheduleStartDate]
                                   ,[AdScheduleEndDate]
                                   ,[RealStep]
                                   ,[AdCategory]
                                   ,[AdKeyWords]
                                   ,[ShowFrequency]
                                   ,[ClickFrequency]
                                   ,[ShowPeriod]
                                   ,[ClickPeriod]
                                   ,[DayBudget]
                                   ,[ProfitValue]
                                   ,[IsPutIn]
                                   ,[AdForm] )
                                   select  
                                   '{0}'
                                   ,'{1}'
                                   ,[Code]
                                   ,[AdvertisersCode]
                                   ,[ActivitiesCode]
                                   ,[Name]
                                   ,[AdMoney]
                                   ,[Remark]
                                   ,[PutInMode]
                                   ,[AdGoalItems]
                                   ,[Status]
                                   ,[Step]
                                   ,[CheckBy]
                                   ,[CheckOn]
                                   ,[AdStartDate]
                                   ,[AdScheduleStartDate]
                                   ,[AdScheduleEndDate]
                                   ,[RealStep]
                                   ,[AdCategory]
                                   ,[AdKeyWords]
                                   ,[ShowFrequency]
                                   ,[ClickFrequency]
                                   ,[ShowPeriod]
                                   ,[ClickPeriod]
                                   ,[DayBudget]
                                   ,[ProfitValue]
                                   ,[IsPutIn]
                                   ,[AdForm]
                                   from DSP_Ads where ID='{2}' or Code='{3}'; ", opCode, 0, ads.ID, ads.Code);
                    }

                    if (opType != (int)AdOperationType.Delete)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_Ads
                                   ([OPSequence]
                                   ,[Type]
                                   ,[Code]
                                   ,[AdvertisersCode]
                                   ,[ActivitiesCode]
                                   ,[Name]
                                   ,[AdMoney]
                                   ,[Remark]
                                   ,[PutInMode]
                                   ,[AdGoalItems]
                                   ,[Status]
                                   ,[Step]
                                   ,[CheckBy]
                                   ,[CheckOn]
                                   ,[AdStartDate]
                                   ,[AdScheduleStartDate]
                                   ,[AdScheduleEndDate]
                                   ,[RealStep]
                                   ,[AdCategory]
                                   ,[AdKeyWords]
                                   ,[ShowFrequency]
                                   ,[ClickFrequency]
                                   ,[ShowPeriod]
                                   ,[ClickPeriod]
                                   ,[DayBudget]
                                   ,[ProfitValue]
                                   ,[IsPutIn]
                                   ,[AdForm])
                                   VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}') ", opCode, 1, ads.Code, ads.AdvertisersCode, ads.ActivitiesCode, ads.Name, ads.AdMoney, ads.Remark, ads.PutInMode, ads.AdGoalItems, ads.Status, ads.Step, ads.CheckBy, ads.CheckOn, ads.AdStartDate, ads.AdScheduleStartDate, ads.AdScheduleEndDate, ads.RealStep, ads.AdCategory, ads.AdKeyWords, ads.ShowFrequency, ads.ClickFrequency, ads.ShowPeriod, ads.ClickPeriod, ads.DayBudget, ads.ProfitValue, ads.IsPutIn,ads.AdForm);
                    }
                    break;
                case "dsp_adtoschedule"://广告排期
                    List<DSP_AdToSchedule> schedule = entity as List<DSP_AdToSchedule>;
                    if (opType != (int)AdOperationType.New)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToSchedule
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[DayOfWeek]
                                   ,[Hours])
                                   select 
                                   '{0}'
                                   ,'{1}'
                                   ,[AdCode]
                                   ,[DayOfWeek]
                                   ,[Hours] 
                                   from DSP_AdToSchedule where AdCode='{2}'; ", opCode, 0, schedule[0].AdCode);
                    }

                    if (opType != (int)AdOperationType.Delete)
                    {
                        foreach (DSP_AdToSchedule item in schedule)
                        {
                            strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToSchedule
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[DayOfWeek]
                                   ,[Hours])
                                   VALUES ('{0}','{1}','{2}','{3}','{4}'); ", opCode, 1, item.AdCode, item.DayOfWeek, item.Hours);
                        }
                    }
                    break;
                case "dsp_adbiddingstrategic"://竞价算法
                    DSP_ADBiddingStrategic adbidding = entity as DSP_ADBiddingStrategic;
                    if (opType != (int)AdOperationType.New)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_ADBiddingStrategic
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[BiddingAlgorithm]
                                   ,[BiddingWay]
                                   ,[BidPrice]
                                   ,[MaxBidPrice])
                                   select 
                                   '{0}'
                                   ,'{1}'
                                   ,[AdCode]
                                   ,[BiddingAlgorithm]
                                   ,[BiddingWay]
                                   ,[BidPrice]
                                   ,[MaxBidPrice]
                                   from DSP_ADBiddingStrategic where AdCode='{2}';", opCode, 0, adbidding.AdCode);
                    }

                    if (opType != (int)AdOperationType.Delete)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_ADBiddingStrategic
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[BiddingAlgorithm]
                                   ,[BiddingWay]
                                   ,[BidPrice]
                                   ,[MaxBidPrice])
                                   VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}') ", opCode, 1, adbidding.AdCode, adbidding.BiddingAlgorithm, adbidding.BiddingWay, adbidding.BidPrice, adbidding.MaxBidPrice);
                    }
                    break;
                case "dsp_adgloableexpressions"://定向表达式
                    DSP_AdGloableExpressions express = entity as DSP_AdGloableExpressions;
                    if (opType != (int)AdOperationType.New)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdGloableExpressions
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[Expressions]
                                   ,[Tags])
                                   select 
                                   '{0}'
                                   ,'{1}'
                                   ,[AdCode]
                                   ,[Expressions]
                                   ,[Tags]
                                   from DSP_AdGloableExpressions  where AdCode='{2}';", opCode, 0, express.AdCode);
                    }

                    if (opType != (int)AdOperationType.Delete)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdGloableExpressions([OPSequence],[Type],[AdCode],[Expressions],[Tags])
                                            select '{0}','{1}','{2}',@exp,'{3}'", opCode, 1, express.AdCode, express.Tags);//express.Expressions

                        sqlParam = new System.Data.SqlClient.SqlParameter("@exp", express.Expressions == null ? string.Empty : express.Expressions);
                    }
                    break;
                case "dsp_adtocreative"://创意信息
                    DSP_AdToCreative creative = entity as DSP_AdToCreative;
                    if (opType != (int)AdOperationType.New)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToCreative
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[Code]
                                   ,[CreativeName]
                                   ,[AdFormsCode]
                                   ,[AdMaterialCode]
                                   ,[ClickEffectType]
                                   ,[Content]
                                   ,[AppPackageName]
                                   ,[State]
                                   ,[Monitor]
                                   ,[ABTest])
                                   select 
                                   '{0}'
                                   ,'{1}'
                                   ,[AdCode]
                                   ,[Code]
                                   ,[CreativeName]
                                   ,[AdFormsCode]
                                   ,[AdMaterialCode]
                                   ,[ClickEffectType]
                                   ,[Content]
                                   ,[AppPackageName]
                                   ,[State]
                                   ,[Monitor]
                                   ,[ABTest]
                                   from DSP_AdToCreative  where AdCode='{2}' and Code='{3}' and IsDeleted=0; ", opCode, 0, creative.AdCode, creative.Code);
                    }

                    if (opType != (int)AdOperationType.Delete)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToCreative
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[Code]
                                   ,[CreativeName]
                                   ,[AdFormsCode]
                                   ,[AdMaterialCode]
                                   ,[ClickEffectType]
                                   ,[Content]
                                   ,[AppPackageName]
                                   ,[State]
                                   ,[Monitor]
                                   ,[ABTest])
                                   select 
                                   '{0}'
                                   ,'{1}'
                                   ,'{2}'
                                   ,'{3}'
                                   ,'{4}'
                                   ,'{5}'
                                   ,'{6}'
                                   ,'{7}'
                                   ,'{8}'
                                   ,'{9}'
                                   ,'{10}'
                                   ,'{11}'
                                   ,'{12}'", opCode, 1, creative.AdCode, creative.Code, creative.CreativeName, creative.AdFormsCode, creative.AdMaterialCode, creative.ClickEffectType, creative.Content, creative.AppPackageName, creative.State, creative.Monitor, creative.ABTest);
                    }
                    break;
                case "dsp_adtoapp":
                    DSP_AdToApp app = entity as DSP_AdToApp;
                    if (opType != (int)AdOperationType.New)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToApp
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[AppCode])
                                   select 
                                   '{0}'
                                   ,'{1}'
                                   ,[AdCode]
                                   ,[AppCode]
                                   from DSP_AdToApp where AdCode='{2}';", opCode, 0, app.AdCode);
                    }

                    if (opType != (int)AdOperationType.Delete)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToApp
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[AppCode])
                                   VALUES ('{0}','{1}','{2}','{3}') ", opCode, 1, app.AdCode, app.AppCode);
                    }
                    break;
                case "dsp_adtoappcontrol":
                    DSP_AdToAppControl control = entity as DSP_AdToAppControl;
                    if (opType != (int)AdOperationType.New)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToAppControl
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[PriceFloat]
                                   ,[IsLocked])
                                   select 
                                   '{0}'
                                   ,'{1}'
                                   ,[AdCode]
                                   ,[PriceFloat]
                                   ,[IsLocked]
                                   from DSP_AdToAppControl where AdCode='{2}';", opCode, 0, control.AdCode);
                    }

                    if (opType != (int)AdOperationType.Delete)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToAppControl
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[PriceFloat]
                                   ,[IsLocked])
                                   VALUES ('{0}','{1}','{2}','{3}','{4}') ", opCode, 1, control.AdCode, control.PriceFloat, control.IsLocked);
                    }
                    break;
                case "dsp_adtoappblacklist":
                    DSP_AdToAppBlackList appBlackList = entity as DSP_AdToAppBlackList;
                    if (opType != (int)AdOperationType.New)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToAppBlackList
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[AppCode])
                                   select 
                                   '{0}'
                                   ,'{1}'
                                   ,[AdCode]
                                   ,[AppCode]
                                   from DSP_AdToAppBlackList where AdCode='{2}';", opCode, 0, appBlackList.AdCode);
                    }

                    if (opType != (int)AdOperationType.Delete)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToAppBlackList
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[AppCode])
                                   VALUES ('{0}','{1}','{2}','{3}') ", opCode, 1, appBlackList.AdCode, appBlackList.AppCode);
                    }
                    break;
                case "dsp_adtodistrict":
                    DSP_AdToDistrict district = entity as DSP_AdToDistrict;
                    if (opType != (int)AdOperationType.New)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToDistrict
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[Name]
                                   ,[Longitude]
                                   ,[Latitude]
                                   ,[Radius]
                                   ,[City])
                                   select 
                                   '{0}'
                                   ,'{1}'
                                   ,[AdCode]
                                   ,[Name]
                                   ,[Longitude]
                                   ,[Latitude]
                                   ,[Radius]
                                   ,[City]
                                   from DSP_AdToDistrict where AdCode='{2}';", opCode, 0, district.AdCode);
                    }

                    if (opType != (int)AdOperationType.Delete)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToDistrict
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[Name]
                                   ,[Longitude]
                                   ,[Latitude]
                                   ,[Radius]
                                   ,[City])
                                   VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}') ", opCode, 1, district.AdCode, district.Name, district.Longitude, district.Latitude, district.Radius,district.City);
                    }
                    break;
                case "dsp_adtoportrait":
                    DSP_AdToPortrait portrait = entity as DSP_AdToPortrait;
                    if (opType != (int)AdOperationType.New)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToPortrait
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[PortraitCode])
                                   select 
                                   '{0}'
                                   ,'{1}'
                                   ,[AdCode]
                                   ,[PortraitCode]
                                   from DSP_AdToPortrait where AdCode='{2}';", opCode, 0, portrait.AdCode);
                    }

                    if (opType != (int)AdOperationType.Delete)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToPortrait
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[PortraitCode])
                                   VALUES ('{0}','{1}','{2}','{3}') ", opCode, 1, portrait.AdCode, portrait.PortraitCode);
                    }
                    break;
                case "dsp_adtooptimize":
                    DSP_AdToOptimize optimize = entity as DSP_AdToOptimize;
                    if (opType != (int)AdOperationType.New)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToOptimize
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[TargetId]
                                   ,[TargetValue]
                                   ,[Intensity]
                                   ,[Hz])
                                   select 
                                   '{0}'
                                   ,'{1}'
                                   ,[AdCode]
                                   ,[TargetId]
                                   ,[TargetValue]
                                   ,[Intensity]
                                   ,[Hz]
                                   from DSP_AdToOptimize where AdCode='{2}';", opCode, 0, optimize.AdCode);
                    }

                    if (opType != (int)AdOperationType.Delete)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToOptimize
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[TargetId]
                                   ,[TargetValue]
                                   ,[Intensity]
                                   ,[Hz])
                                   VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}') ", opCode, 1, optimize.AdCode, optimize.TargetId, optimize.TargetValue, optimize.Intensity, optimize.Hz);
                    }
                    break;
                case "dsp_adtoscenedistrict":
                    DSP_AdToSceneDistrict sDistrict = entity as DSP_AdToSceneDistrict;
                    if (opType != (int)AdOperationType.New)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToSceneDistrict
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[DistrictCode]
                                   ,[SceneName])
                                   select 
                                   '{0}'
                                   ,'{1}'
                                   ,[AdCode]
                                   ,[DistrictCode]
                                   ,[SceneName]
                                   from DSP_AdToSceneDistrict where AdCode='{2}';", opCode, 0, sDistrict.AdCode);
                    }

                    if (opType != (int)AdOperationType.Delete)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToSceneDistrict
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[DistrictCode]
                                   ,[SceneName])
                                   VALUES ('{0}','{1}','{2}','{3}','{4}') ", opCode, 1, sDistrict.AdCode, sDistrict.DistrictCode, sDistrict.SceneName);
                    }
                    break;
                case "dsp_adtovisitorgroup":
                    DSP_AdToVisitorGroup visitorGroup = entity as DSP_AdToVisitorGroup;
                    if (opType != (int)AdOperationType.New)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToVisitorGroup
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[VisitorGroupCode]
                                   ,[Related])
                                   select 
                                   '{0}'
                                   ,'{1}'
                                   ,[AdCode]
                                   ,[VisitorGroupCode]
                                   ,[Related]
                                   from DSP_AdToVisitorGroup where AdCode='{2}';", opCode, 0, visitorGroup.AdCode);
                    }

                    if (opType != (int)AdOperationType.Delete)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToVisitorGroup
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[VisitorGroupCode]
                                   ,[Related])
                                   VALUES ('{0}','{1}','{2}','{3}','{4}') ", opCode, 1, visitorGroup.AdCode, visitorGroup.VisitorGroupCode, visitorGroup.Related);
                    }
                    break;
                case "dsp_adtospecificArea":
                    DSP_AdToSpecificArea specificArea = entity as DSP_AdToSpecificArea;
                    if (opType != (int)AdOperationType.New)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToSpecificArea
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[ProvinceCode]
                                   ,[CityCode]
                                   ,[ProvinceName]
                                   ,[CityName]
                                   ,[CountyCode]
                                   ,[CountyName])
                                   select 
                                   '{0}'
                                   ,'{1}'
                                   ,[AdCode]
                                   ,[ProvinceCode]
                                   ,[CityCode]
                                   ,[ProvinceName]
                                   ,[CityName]
                                   ,[CountyCode]
                                   ,[CountyName]
                                   from DSP_AdToSpecificArea where AdCode='{2}';", opCode, 0, specificArea.AdCode);
                    }

                    if (opType != (int)AdOperationType.Delete)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_AdToSpecificArea
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[ProvinceCode]
                                   ,[CityCode]
                                   ,[ProvinceName]
                                   ,[CityName]
                                   ,[CountyCode]
                                   ,[CountyName])
                                   VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}') ", opCode, 1, specificArea.AdCode, specificArea.ProvinceCode, specificArea.CityCode, specificArea.ProvinceName, specificArea.CityName, specificArea.CountyCode, specificArea.CountyName);
                    }
                    break;
                case "dsp_adtopeoplekeywords":
                    DSP_ADToPeopleKeywords peopleKeywords = entity as DSP_ADToPeopleKeywords;

                    if (opType != (int)AdOperationType.New)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_ADToPeopleKeywords
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[KeyWords])
                                   select 
                                   '{0}'
                                   ,'{1}'
                                   ,[AdCode]
                                   ,[KeyWords]
                                   from DSP_ADToPeopleKeywords where AdCode='{2}';", opCode, 0, peopleKeywords.AdCode);
                    }

                    if (opType != (int)AdOperationType.Delete)
                    {
                        strSQL += string.Format(@" INSERT INTO AdOperationLog_DSP_ADToPeopleKeywords
                                   ([OPSequence]
                                   ,[Type]
                                   ,[AdCode]
                                   ,[KeyWords])
                                   VALUES ('{0}','{1}','{2}','{3}') ", opCode, 1, peopleKeywords.AdCode, peopleKeywords.KeyWords);
                    }
                    break;
                default:
                    break;
            }

            //执行数据库操作
            if (strSQL.Trim() != string.Empty)
            {
                if (sqlParam == null)
                    DB.Database.ExecuteSqlCommand(strSQL, string.Empty);
                else
                    DB.Database.ExecuteSqlCommand(strSQL, sqlParam);
            }
        }
    }
}
