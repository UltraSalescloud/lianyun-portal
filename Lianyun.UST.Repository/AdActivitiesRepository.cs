using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Model.Business_Entities;
using Lianyun.UST.Model.Lianyun_UST_Entities;
using Lianyun.UST.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Repository
{
    /// <summary>
    /// 广告计划仓储实现
    /// </summary>
    public class AdActivitiesRepository : BaseRepository<DSP_AdActivities>, IAdActivitiesRepository
    {
        public AdActivitiesRepository(Lianyun_DSPContext lianyun_DSPContext, ILogger logger) : base(lianyun_DSPContext,logger) { }

        /// <summary>
        /// 通过创建者AdvertisersCode获取广告计划
        /// </summary>
        /// <param name="AdvertisersCode"></param>
        /// <returns></returns>
        public List<DSP_AdActivitiesChild> GetAdActivitiesByAdvertisersCode(string advCode)
        {
            string sql = string.Empty;

            sql = @" SELECT     * , PayMoney = TotalMoney
                                - (SELECT ISNULL(SUM(AdMoney),0) FROM dbo.DSP_Ads b WHERE ActivitiesCode=a.code AND b.AdvertisersCode=a.AdvertisersCode AND IsDelete=0)
                                + (select ISNULL(SUM(AdMoney),0)-ISNULL(SUM(PayMoney),0) from DSP_AdPurchaseRecord c where c.AdvertisersCode=a.AdvertisersCode and c.AdCode in (select code from DSP_Ads where DSP_Ads.ActivitiesCode=a.code AND DSP_Ads.IsDelete=0))             
                     FROM       DSP_AdActivities a
                     WHERE      AdvertisersCode = @AdvertisersCode AND IsDeleted=0
                     ORDER BY CreatedOn DESC ";

            var queryResult = DB.Database.SqlQuery<DSP_AdActivitiesChild>(sql, new SqlParameter("@AdvertisersCode", advCode));
            return queryResult.ToList();
        }

        /// <summary>
        /// 获取广告计划剩余金额
        /// </summary>
        /// <param name="adActivities"></param>
        /// <returns></returns>
        public DSP_AdActivitiesChild GetAdActivitiesByCode(string code)
        {
            string sql = string.Empty;

            sql = @"select  *, PayMoney = TotalMoney
                            - (select ISNULL(SUM(AdMoney),0) from DSP_Ads b where b.ActivitiesCode=a.code AND b.IsDelete=0) 
                            + (select ISNULL(SUM(AdMoney),0)-ISNULL(SUM(PayMoney),0) from DSP_AdPurchaseRecord c where c.AdvertisersCode=a.AdvertisersCode and c.AdCode in (select code from DSP_Ads where DSP_Ads.ActivitiesCode=a.code AND DSP_Ads.IsDelete=0)) 
                    from    DSP_AdActivities a 
                    where   Code = @Code ";

            var queryResult = DB.Database.SqlQuery<DSP_AdActivitiesChild>(sql, new SqlParameter("@Code", code));
            return queryResult.FirstOrDefault();
        }

        /// <summary>
        /// 获取广告主剩余总金额
        /// </summary>
        /// <param name="adActivities"></param>
        /// <returns></returns>
        public DSP_AdActivitiesChild GetPayMoneyByAdvCode(string code)
        {
            string sql = string.Empty;

            //            sql = @" SELECT PayMoney=ISNULL(SUM(Money),0)
            //                            - (select ISNULL(SUM(TotalMoney),0) from DSP_AdActivities  where AdvertisersCode=@AdvertisersCode AND IsDeleted=0)
            //	                        + (select ISNULL(SUM(AdMoney),0)-ISNULL(SUM(PayMoney),0) from DSP_AdPurchaseRecord where AdvertisersCode=@AdvertisersCode)
            //                        FROM DSP_AdvTradeRecord WHERE TradeType=1 AND AdvertisersCode= @AdvertisersCode ";

            sql = @"SELECT  PayMoney=ISNULL(SUM(Money),0)
                            - (select ISNULL(SUM(TotalMoney),0) from DSP_AdActivities where AdvertisersCode=@AdvertisersCode AND IsDeleted=0)
                    FROM    DSP_AdvTradeRecord
                    WHERE   TradeType=1 AND AdvertisersCode=@AdvertisersCode
                    ";

            var queryResult = DB.Database.SqlQuery<DSP_AdActivitiesChild>(sql, new SqlParameter("@AdvertisersCode", code));
            return queryResult.FirstOrDefault();
        }

        //public DSP_AdActivities GetAdActivities(DSP_AdActivities model)
        //{
        //    using (var _db = new Lianyun_DSPContext())
        //    {
        //        return _db.DSP_AdActivities.FirstOrDefault(x => x.Code == model.Code && x.CreatedBy == model.CreatedBy);
        //    }
        //}



        public DSP_AdActivitiesExt GetHomePageHeaderData(string AdvertisersCode)
        {
            string sql = string.Empty;
            sql = @"SELECT  Pending = ISNULL(SUM(CASE Status WHEN 2 THEN 1 ELSE 0 end),0),
                            UnPassed = ISNULL(SUM(CASE WHEN Status=1 AND Step=6 AND CheckBy IS NOT NULL AND LEN(CheckBy)>0 THEN 1 ELSE 0 end),0),
                            PutIning = ISNULL(SUM(CASE WHEN Status =3 THEN 1 ELSE 0 end),0),
                            Paused = ISNULL(SUM(CASE WHEN Status=4 THEN 1 ELSE 0 end),0),
                            Balance = (SELECT ISNULL(SUM(Money),0) FROM DSP_AdvTradeRecord WHERE TradeType=1 AND AdvertisersCode = @AdvertisersCode ) 
                                    - (select ISNULL(SUM(PayMoney),0) from DSP_AdPurchaseRecord where AdvertisersCode = @AdvertisersCode),
                            BalanceBlocked = (Select ISNULL(SUM(TotalMoney),0) From DSP_AdActivities Where IsDeleted=0 And AdvertisersCode = @AdvertisersCode)
                                    - (select ISNULL(SUM(PayMoney),0) from DSP_AdPurchaseRecord where AdvertisersCode = @AdvertisersCode) 
                    FROM    dbo.DSP_Ads 
                    WHERE   IsDelete=0 AND AdvertisersCode=@AdvertisersCode";

            var queryResult=DB.Database.SqlQuery<DSP_AdActivitiesExt>(sql, new SqlParameter("@AdvertisersCode", AdvertisersCode));
            return queryResult.FirstOrDefault();
        }
    }
}
