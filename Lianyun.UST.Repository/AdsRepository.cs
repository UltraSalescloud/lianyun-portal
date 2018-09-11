using System.Data;
using System.Security.AccessControl;
using System.Security.Policy;
using AutoMapper.Mappers;
using Lianyun.UST.Infrastructure.Enums;
using Lianyun.UST.Model.Lianyun_UST_Entities;
using Lianyun.UST.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Lianyun.UST.Infrastructure;
using Lianyun.UST.Infrastructure.Logging;

namespace Lianyun.UST.Repository
{
    /// <summary>
    /// 广告策略仓储实现
    /// </summary>
    public class AdsRepository : BaseRepository<DSP_Ads>, IAdsRepository
    {
        public AdsRepository(Lianyun_DSPContext lianyun_DSPContext, ILogger logger) : base(lianyun_DSPContext, logger) { }

        public bool CopyAds(string code, string userCode)
        {
            bool result = false;

            string strSQL = @"exec [Lianyun_DSP].[dbo].[Pro_CopySingleAds] @Code,@UserCode,@Message OUTPUT";

            SqlParameter[] paramList = new SqlParameter[]{
                new SqlParameter("@Code", code),
                new SqlParameter("@UserCode", userCode),
                new SqlParameter("@Message",string.Empty)
            };

            paramList[2].Direction = System.Data.ParameterDirection.Output;

            DB.Database.ExecuteSqlCommand(strSQL, paramList);
            result = !paramList[2].Value.ToString().IsNotEmpty();
            return result;
        }

        public string GetCategory()
        {
            string strJson = string.Empty;
            string strSQL = @"
                --Declare @json nvarchar(max)
                Set		@json = ''
                select	@json =@json+',{""id"":'+c.CategoryID+',""name"":""'+c.Name+'"",""child"":['+(Case When c.child IS NULL THEN '{""id"":""'+c.CategoryID+'"",""name"":""'+c.name+'""},' ELSE c.child END)+']}'
                from(
		                select	a.CategoryID,a.Name,
				                stuff((	
					                select  ',{""id"":""'+b.CategoryID+'"",""name"":""'+b.Name+'""}'
					                from	[Lianyun].dbo.LocalAdCategory b
					                where   b.FatherClass=a.CategoryID
					                for XML PATH('')),1,1,'') child
		                from	[Lianyun].dbo.LocalAdCategory a
		                where	a.FatherClass=0 and isdeleted=0 
                )c
        
                Select	@json = stuff(@json,1,1,'')";

            SqlParameter[] paramList = new SqlParameter[]{
                new SqlParameter("@json", System.Data.SqlDbType.NVarChar,Int32.MaxValue)
            };

            paramList[0].Direction = System.Data.ParameterDirection.Output;

            DB.Database.ExecuteSqlCommand(strSQL, paramList);
            strJson = paramList[0].Value.ToString();
            return "[" + strJson + "]";
        }


        public string GetAllCategory()
        {
            string strJson = string.Empty;
            string strSQL = @"
                --Declare @json nvarchar(max)
                Set		@json = ''
                select	@json = @json+',{""id"":'+c.CategoryID+',""name"":""'+c.Name+'"",""child"":['+(Case When c.child IS NULL THEN '{""id"":""'+c.CategoryID+'"",""name"":""'+c.name+'""},' ELSE c.child END)+']}'
                from(
		                select	a.CategoryID,a.Name,
				                stuff((	
					                select  ',{""id"":""'+b.CategoryID+'"",""name"":""'+b.Name+'""}'
					                from	[Lianyun].dbo.LocalAdCategory b
					                where   b.FatherClass=a.CategoryID
					                for XML PATH('')),1,1,'') child
		                from	[Lianyun].dbo.LocalAdCategory a
		                where	a.FatherClass=0 
                )c
        
                Select	@json = stuff(@json,1,1,'')";

            SqlParameter[] paramList = new SqlParameter[]{
                new SqlParameter("@json", System.Data.SqlDbType.NVarChar,Int32.MaxValue)
            };

            paramList[0].Direction = System.Data.ParameterDirection.Output;

            DB.Database.ExecuteSqlCommand(strSQL, paramList);
            strJson = paramList[0].Value.ToString();
            return "[" + strJson + "]";
        }


        public decimal GetProfit(string userCode)
        {
            decimal ret = 0;
            if (userCode.IsNotEmpty())
            {
                string sql = string.Format(" DECLARE @p DECIMAL=5 ;SELECT @p=ProfitValue FROM dbo.DSP_AdvertiserProfit WHERE AdvertisersCode='{0}';SELECT @p ", userCode);
                object obj = DB.Database.SqlQuery<decimal>(sql, "").FirstOrDefault();
                decimal.TryParse(obj.ToString(), out ret);
            }
            return ret;
        }

        /// <summary>
        /// 得到广告的列表 
        /// </summary>
        /// <param name="adCode">广告主的ID，通过此过滤</param>
        /// <returns></returns>
        public List<DSP_Ads> GetAdsList(string adCode)//int status, DateTime startTime, DateTime endTime)
        {
            int sta = (int)AdsStatusEnum.End;
            DateTime startAdTime = DateTime.Now.AddDays(-180);
            DateTime endAdTime = DateTime.Now;
            List<DSP_Ads> adsList = DbSet.Where(x => x.Status == sta && x.AdvertisersCode == adCode && x.AdScheduleStartDate.Value >= startAdTime && x.AdScheduleEndDate <= endAdTime && x.IsDelete == false).ToList();
            return adsList;
        }

        public DSP_Ads GetAdsById(int id)
        {
            DSP_Ads ads = DbSet.Find(id);
            return ads;
        }


        public bool CheckIsABTestEnabled(string code)
        {
            string sql = " SELECT top 1 * FROM dsp_ads WHERE  EXISTS( SELECT TOP 1 id FROM dbo.DSP_AdToCreative WHERE AdCode=@AdCode AND abTest=1 AND IsDeleted ='0')";
            List<DSP_Ads> lst= DB.Database.SqlQuery<DSP_Ads>(sql, new SqlParameter("@AdCode", code)).ToList();

            return lst.Count > 0;
        }

        public List<DSP_AdsChild> GetAdsListByActivitiesCode(string actCode)
        {
            string sql = string.Empty;

            sql = @" SELECT     a.*,c.* ,
                                ConsumptionAmount = Case When ISNULL(Amount,0) <= a.AdMoney Then ISNULL(Amount,0) Else a.AdMoney End, t.Timestamp            
                     FROM       DSP_Ads a Left Join DSP_ADConsumptionAmount c On c.AdCode=a.Code
                        LEFT JOIN dbo.DSP_AdsTimestamp t ON a.Code = t.AdCode
                     WHERE      ActivitiesCode = @ActivitiesCode AND IsDelete=0
                     ORDER BY CreatedOn DESC ";

            var queryResult = DB.Database.SqlQuery<DSP_AdsChild>(sql, new SqlParameter("@ActivitiesCode", actCode));
            return queryResult.ToList();
        }

        /// <summary>
        /// 获取广告信息（包括已消费金额）
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DSP_AdsChild GetAdsByCode(string code)
        {
            string sql = string.Empty;

            sql = @" SELECT     * ,
                                ConsumptionAmount = Case When ISNULL(Amount,0) <= a.AdMoney Then ISNULL(Amount,0) Else a.AdMoney End             
                     FROM       DSP_Ads a Left Join DSP_ADConsumptionAmount c On c.AdCode=a.Code
                     WHERE      Code = @Code AND IsDelete=0";

            var queryResult = DB.Database.SqlQuery<DSP_AdsChild>(sql, new SqlParameter("@Code", code));
            return queryResult.FirstOrDefault<DSP_AdsChild>();
        }
    }
}
