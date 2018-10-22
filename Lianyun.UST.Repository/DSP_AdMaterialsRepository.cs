
using Lianyun.UST.Infrastructure.Enums;
using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Model;
using Lianyun.UST.Model.Entities;
using Lianyun.UST.Model.Lianyun_UST_Entities;
using Lianyun.UST.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lianyun.UST.Repository
{
    public class DSP_AdMaterialsRepository : BaseRepository<DSP_AdMaterials>, IDSP_AdMaterialsRepository
    {
        public DSP_AdMaterialsRepository(Lianyun_DSPContext lianyun_DSPContext, ILogger logger) : base(lianyun_DSPContext,logger) { }

        public List<DSP_AdMaterials> GetAllList()
        {
            var lst_dspam = this.GetListBy(o => o.IsDelete == false);
            return lst_dspam.ToList();
        }

        public List<AdMaterialsBM> GetAdMaterialsBMListBy(string RequestType, string RequestValue, string AdvertisersCode)
        {
            string sql = @"SELECT
                                a.* ,
                                d.DictionaryItemName CatagoryName,
                                s.ImageUrl ImgUrl,
                                v.IconUrl IconUrl
                            FROM
                                DSP_AdMaterials a
                            LEFT JOIN (
								SELECT T1.MaterialCode,LEFT(ImgUrl,LEN(ImgUrl)-1) AS ImageUrl FROM (
								SELECT MaterialCode,(
									SELECT ImageUrl + ';'
									FROM dbo.DSP_MaterialToImages 
									WHERE MaterialCode = T.MaterialCode FOR XML PATH('')) ImgUrl
								FROM dbo.DSP_MaterialToImages T
								GROUP BY T.MaterialCode
								)T1
                            ) s
                                ON a.Code = s.MaterialCode
                            LEFT JOIN DSP_MaterialToVideo v
                                ON a.Code = v.MaterialCode
                            LEFT JOIN [Lianyun].dbo.DataDictionaryItem d
                            ON a.Catagory = DictionaryItemValue
                            WHERE IsDelete = 0 AND a.IsHistoryMark = 0 AND d.DataDictionaryCategoryID = 26 and a.AdvertisersCode=@AdvertisersCode
                                                                                    ";
           
            List<AdMaterialsBM> lst_dsp_atc = new List<AdMaterialsBM>();

            if (!string.IsNullOrEmpty(RequestValue))
            {
                if (RequestType == ((int)AdMaterialsSearchTypeEnum.MaterialsName).ToString())
                {
                    sql += " And a.Name like '%'+@Name+'%' ";
                    sql += " ORDER BY a.ModifiedOn DESC ";
                    lst_dsp_atc = this.DB.Database.SqlQuery<AdMaterialsBM>(sql, new SqlParameter("@AdvertisersCode", AdvertisersCode), new SqlParameter("@Name", RequestValue)).ToList();
                }
                else if (RequestType == ((int)AdMaterialsSearchTypeEnum.MaterialsType).ToString())
                {
                    sql += " And d.DictionaryItemName like '%'+@CatagoryName+'%' ";
                    sql += " ORDER BY a.ModifiedOn DESC ";
                    lst_dsp_atc = this.DB.Database.SqlQuery<AdMaterialsBM>(sql, new SqlParameter("@AdvertisersCode", AdvertisersCode), new SqlParameter("@CatagoryName", RequestValue)).ToList();
                }
                else
                {
                    sql += " ORDER BY a.ModifiedOn DESC ";
                    lst_dsp_atc = this.DB.Database.SqlQuery<AdMaterialsBM>(sql, new SqlParameter("@AdvertisersCode", AdvertisersCode)).ToList();
                }

            }
            else
            {
                sql += " ORDER BY a.ModifiedOn DESC ";
                lst_dsp_atc = this.DB.Database.SqlQuery<AdMaterialsBM>(sql, new SqlParameter("@AdvertisersCode", AdvertisersCode)).ToList();
            }



            return lst_dsp_atc;
        }

        public List<AdMaterialsBM> GetAdMaterialsBMListBy(string RequestType, string RequestValue, string AdvertisersCode, int AdForm)
        {
            string tep = AdForm < (int)MaterialsTypeEnum.Video ? " in (1,2,3) " : " = " + AdForm;

            string sql = @"SELECT
                                a.* ,
                                d.DictionaryItemName CatagoryName,
                                s.ImageUrl ImgUrl,
                                v.IconUrl IconUrl
                            FROM
                                DSP_AdMaterials a
                            LEFT JOIN (
								SELECT T1.MaterialCode,LEFT(ImgUrl,LEN(ImgUrl)-1) AS ImageUrl FROM (
								SELECT MaterialCode,(
									SELECT ImageUrl + ';'
									FROM dbo.DSP_MaterialToImages 
									WHERE MaterialCode = T.MaterialCode FOR XML PATH('')) ImgUrl
								FROM dbo.DSP_MaterialToImages T
								GROUP BY T.MaterialCode
								)T1
                            ) s
                                ON a.Code = s.MaterialCode
                            LEFT JOIN DSP_MaterialToVideo v
                                ON a.Code = v.MaterialCode
                            LEFT JOIN [Lianyun].dbo.DataDictionaryItem d
                            ON a.Catagory = DictionaryItemValue
                            WHERE IsDelete = 0 AND a.IsHistoryMark = 0 AND a.Catagory " + tep + " AND d.DataDictionaryCategoryID = 26 and a.AdvertisersCode=@AdvertisersCode";

            List<AdMaterialsBM> lst_dsp_atc = new List<AdMaterialsBM>();

            if (!string.IsNullOrEmpty(RequestValue))
            {
                if (RequestType == ((int)AdMaterialsSearchTypeEnum.MaterialsName).ToString())
                {
                    sql += " And a.Name like '%'+@Name+'%' ";
                    sql += " ORDER BY a.ModifiedOn DESC ";
                    lst_dsp_atc = this.DB.Database.SqlQuery<AdMaterialsBM>(sql, new SqlParameter("@AdvertisersCode", AdvertisersCode), new SqlParameter("@Name", RequestValue)).ToList();
                }
                else if (RequestType == ((int)AdMaterialsSearchTypeEnum.MaterialsType).ToString())
                {
                    sql += " And d.DictionaryItemName like '%'+@CatagoryName+'%' ";
                    sql += " ORDER BY a.ModifiedOn DESC ";
                    lst_dsp_atc = this.DB.Database.SqlQuery<AdMaterialsBM>(sql, new SqlParameter("@AdvertisersCode", AdvertisersCode), new SqlParameter("@CatagoryName", RequestValue)).ToList();
                }
                else
                {
                    sql += " ORDER BY a.ModifiedOn DESC ";
                    lst_dsp_atc = this.DB.Database.SqlQuery<AdMaterialsBM>(sql, new SqlParameter("@AdvertisersCode", AdvertisersCode)).ToList();
                }

            }
            else
            {
                sql += " ORDER BY a.ModifiedOn DESC ";
                lst_dsp_atc = this.DB.Database.SqlQuery<AdMaterialsBM>(sql, new SqlParameter("@AdvertisersCode", AdvertisersCode)).ToList();
            }



            return lst_dsp_atc;
        }

        public List<AdMaterialsBM> GetAdMaterialsBMListBy(string AdvertisersCode, List<string> sMaterialsCode)
        {
            string sCodes = string.Empty;

            sMaterialsCode.ForEach(delegate(string code)
            {
                sCodes += "'" + code + "',";
            });

            string sql = @"SELECT
                                a.* ,
                                d.DictionaryItemName CatagoryName,
                                s.ImageUrl ImgUrl
                            FROM
                                DSP_AdMaterials a
                            LEFT JOIN (
								SELECT T1.MaterialCode,LEFT(ImgUrl,LEN(ImgUrl)-1) AS ImageUrl FROM (
								SELECT MaterialCode,(
									SELECT ImageUrl + ';'
									FROM dbo.DSP_MaterialToImages 
									WHERE MaterialCode = T.MaterialCode FOR XML PATH('')) ImgUrl
								FROM dbo.DSP_MaterialToImages T
								GROUP BY T.MaterialCode
								)T1
                            ) s
                                ON a.Code = s.MaterialCode
                                LEFT JOIN [Lianyun].dbo.DataDictionaryItem d
                                ON a.Catagory = DictionaryItemValue
                            WHERE IsDelete = 0 AND a.IsHistoryMark = 0 AND d.DataDictionaryCategoryID = 26 and a.AdvertisersCode=@AdvertisersCode
                                  and a.code in (" + sCodes.TrimEnd(',') + ") ";
            sql += " ORDER BY a.ModifiedOn DESC ";

            List<AdMaterialsBM> lst_dsp_atc = new List<AdMaterialsBM>();

            lst_dsp_atc = this.DB.Database.SqlQuery<AdMaterialsBM>(sql, new SqlParameter("@AdvertisersCode", AdvertisersCode)).ToList();

            return lst_dsp_atc;
        }

        public bool DeletedValidate(string sMaterialCode)
        {
            string sql = @"
                            SELECT 1 FROM dbo.DSP_AdToCreative T1
                            LEFT JOIN dbo.DSP_Ads T2 ON T1.AdCode = T2.Code
                            WHERE CHARINDEX(AdMaterialCode,'" + sMaterialCode + "') > 0 AND T1.IsDeleted = 0 AND T2.IsDelete = 0 ";

            return this.DB.Database.SqlQuery<int>(sql).FirstOrDefault() == 0;
        }
    }
}
