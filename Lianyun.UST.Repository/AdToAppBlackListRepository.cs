using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Model.Lianyun_UST_Entities;
using Lianyun.UST.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Repository
{
    public class AdToAppBlackListRepository:BaseRepository<DSP_AdToAppBlackList>,IAdToAppBlackListRepository
    {
        public AdToAppBlackListRepository(Lianyun_DSPContext lianyun_DSPContext, ILogger logger) : base(lianyun_DSPContext, logger) { }

        public string GetBlackAppList(string adCode) {
            string strJson = string.Empty;
            string strSQL = @"--declare @json varchar(300)
                        set @json=''
                        SELECT @json=@json+'{""Prefix"":""'+ CASE WHEN c.Name IS NULL THEN '' ELSE c.Name + ' - ' END + '"",""Value"":""' + b.Code + '"",""Name"":""' + AppName + '""},' 
                        FROM [Lianyun_DSP].dbo.[DSP_AdToAppBlackList] a LEFT JOIN [Lianyun].dbo.[LomarkApps] b ON a.AppCode=b.Code 
                        LEFT JOIN [Lianyun].dbo.[LocalAppTypeLib] c ON b.AppTypeCode = c.Code
                        WHERE b.IsDeleted=0 AND a.AdCode=@Value
                        select @json";

            SqlParameter[] paramList = new SqlParameter[]{
                new SqlParameter("@Value",System.Data.SqlDbType.NVarChar,50),
                new SqlParameter("@json", System.Data.SqlDbType.NVarChar,Int32.MaxValue)
            };

            paramList[0].Value = adCode;
            paramList[1].Direction = System.Data.ParameterDirection.Input;
            paramList[1].Direction = System.Data.ParameterDirection.Output;

            DB.Database.ExecuteSqlCommand(strSQL, paramList);
            strJson = paramList[1].Value.ToString();
            return "[" + strJson.TrimEnd(',') + "]";
        }
    }
}
