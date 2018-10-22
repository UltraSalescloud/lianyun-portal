using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Infrastructure.Page;
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
    /// 充值记录仓储层
    /// </summary>
    public class AdvTradeRepository : BaseRepository<DSP_AdActivities>, IAdvTradeRepository
    {
        public AdvTradeRepository(Lianyun_DSPContext lianyun_DSPContext,ILogger logger) : base(lianyun_DSPContext,logger) { }

        public List<DSP_AdvTradeRecord> GetRecordsByAdvCode(string advCode)
        {
            string sql = string.Empty;
            sql = @" select * from DSP_AdvTradeRecord where AdvertisersCode=@AdvertisersCode and TradeType=@TradeType order by TradeDate desc";
            List<DSP_AdvTradeRecord> modelList = DB.Database.SqlQuery<DSP_AdvTradeRecord>(sql, new SqlParameter("@AdvertisersCode", advCode), new SqlParameter("@TradeType", 1)).ToList();
            return modelList;
        }


        public List<DSP_AdvTradeRecord> GetRecordsAjaxPage(DSP_AdvTradeRecord condition, int PageIndex, int PageSize, out int Total)
        {
            string sql = string.Empty;
            sql = @" select * from DSP_AdvTradeRecord where AdvertisersCode=@AdvertisersCode and TradeType=@TradeType ";
            StringBuilder sbSubSelect = new StringBuilder();
            sbSubSelect.Append("(");
            sbSubSelect.Append(sql);
            sbSubSelect.Append(" ) d ");
            string Order = " TradeDate desc";
            //定义排序字段
            string sOrder = Order;
            //查找所有列
            string sSelect = " *";
            //生成分页sql语句
            string sPageSql = BuildPagerSqlHelper.BuildPagerDateSetSql(PageSize, PageIndex, sSelect, sbSubSelect.ToString(), sOrder);
            List<DSP_AdvTradeRecord> modelList = DB.Database.SqlQuery<DSP_AdvTradeRecord>(sql, new SqlParameter("@AdvertisersCode", condition.AdvertisersCode), new SqlParameter("@TradeType", 1)).ToList();
            Total = modelList == null ? 0 : modelList.Count;
            List<DSP_AdvTradeRecord> PageList = DB.Database.SqlQuery<DSP_AdvTradeRecord>(sPageSql, new SqlParameter("@AdvertisersCode", condition.AdvertisersCode), new SqlParameter("@TradeType", 1)).ToList();
            return PageList;
        }
    }
}
