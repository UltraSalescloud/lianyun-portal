using Lianyun.UST.Infrastructure.Enums;
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
    public class DSP_AdToCreativeRepository : BaseRepository<DSP_AdToCreative>, IDSP_AdToCreativeRepository
    {
        public DSP_AdToCreativeRepository(Lianyun_DSPContext lianyun_DSPContext, ILogger logger) : base(lianyun_DSPContext,logger) { }

        public List<AdToCreativeBM> GetAllList(string adCode)
        {
            string sql = @"SELECT
                                distinct
                                c.* ,
                                m.Catagory,
                                d.DictionaryItemName CatagoryName,
                                s.ImageUrl ImgUrl,
                                f.Name AdFormsName,
                                f.AdSpaceType
                            FROM DSP_AdToCreative c
                            LEFT JOIN [Lianyun].dbo.AdForms f ON f.Code = c.AdFormsCode
                            LEFT JOIN dbo.DSP_AdMaterials m ON c.AdMaterialCode = m.Code
                            LEFT JOIN (
								SELECT T1.MaterialCode,LEFT(ImgUrl,LEN(ImgUrl)-1) AS ImageUrl FROM (
									SELECT MaterialCode,(
										SELECT ImageUrl + ';'
										FROM dbo.DSP_MaterialToImages 
										WHERE MaterialCode = T.MaterialCode FOR XML PATH('')) ImgUrl
									FROM dbo.DSP_MaterialToImages T
									GROUP BY MaterialCode
								)T1
                            ) s ON c.AdMaterialCode = s.MaterialCode 
                            LEFT JOIN [Lianyun].dbo.DataDictionaryItem d ON m.Catagory = DictionaryItemValue  
                            WHERE c.AdCode = @AdCode AND f.AdSpaceType <> @type AND IsDeleted = @IsDeleted AND d.DataDictionaryCategoryID = 26 
                        UNION
                            SELECT
                                distinct
                                c.* ,
                                m.Catagory,
                                d.DictionaryItemName CatagoryName,
                                s.IconUrl ImgUrl,
                                f.Name AdFormsName,
                                f.AdSpaceType
                            FROM DSP_AdToCreative c
                            LEFT JOIN [Lianyun].dbo.AdForms f ON f.Code = c.AdFormsCode
                            LEFT JOIN dbo.DSP_AdMaterials m ON c.AdMaterialCode = m.Code
                            LEFT JOIN dbo.DSP_MaterialToVideo s ON c.AdMaterialCode = s.MaterialCode 
                            LEFT JOIN [Lianyun].dbo.DataDictionaryItem d ON m.Catagory = DictionaryItemValue 
                            WHERE c.AdCode = @AdCode AND f.AdSpaceType = @type AND IsDeleted = @IsDeleted AND d.DataDictionaryCategoryID = 26 order by ModifiedOn";
            return this.DB.Database.SqlQuery<AdToCreativeBM>(sql, new SqlParameter("@AdCode", adCode), new SqlParameter("@IsDeleted", false), new SqlParameter("@type", (int)MaterialsTypeEnum.Video)).ToList();
        }

        public List<AdToCreativeBM> GetApprovedList(string adCode)
        {
            string sql = @"SELECT
                                distinct
                                c.* ,
                                m.Catagory,
                                d.DictionaryItemName CatagoryName,
                                s.ImageUrl ImgUrl,
                                f.Name AdFormsName
                            FROM DSP_AdToCreative c
                            LEFT JOIN [Lianyun].dbo.AdForms f ON f.Code = c.AdFormsCode
                            LEFT JOIN dbo.DSP_AdMaterials m ON c.AdMaterialCode = m.Code
                            LEFT JOIN (
								SELECT T1.MaterialCode,LEFT(ImgUrl,LEN(ImgUrl)-1) AS ImageUrl FROM (
									SELECT MaterialCode,(
										SELECT ImageUrl + ';'
										FROM dbo.DSP_MaterialToImages 
										WHERE MaterialCode = T.MaterialCode FOR XML PATH('')) ImgUrl
									FROM dbo.DSP_MaterialToImages T
									GROUP BY MaterialCode
								)T1
                            ) s ON c.AdMaterialCode = s.MaterialCode 
                            LEFT JOIN [Lianyun].dbo.DataDictionaryItem d ON m.Catagory = DictionaryItemValue 
                            WHERE c.AdCode = @AdCode AND f.AdSpaceType <> @type and len(c.content) > 0 and c.state <> " + (int)CreativeStatusEnum.Draft + " AND IsDeleted = @IsDeleted AND d.DataDictionaryCategoryID = 26  " +
                        " UNION SELECT distinct  c.* ,m.Catagory, d.DictionaryItemName CatagoryName, s.IconUrl ImgUrl, f.Name AdFormsName " +
                         "    FROM DSP_AdToCreative c " +
                          "   LEFT JOIN [Lianyun].dbo.AdForms f ON f.Code = c.AdFormsCode " +
                          "   LEFT JOIN dbo.DSP_AdMaterials m ON c.AdMaterialCode = m.Code " +
                          "   LEFT JOIN dbo.DSP_MaterialToVideo s ON c.AdMaterialCode = s.MaterialCode  " +
                          "   LEFT JOIN [Lianyun].dbo.DataDictionaryItem d ON m.Catagory = DictionaryItemValue WHERE c.AdCode = @AdCode AND f.AdSpaceType = @type and len(c.content) > 0 and c.state <> " + (int)CreativeStatusEnum.Draft + " AND IsDeleted = @IsDeleted AND d.DataDictionaryCategoryID = 26 order by ModifiedOn desc";
            return this.DB.Database.SqlQuery<AdToCreativeBM>(sql, new SqlParameter("@AdCode", adCode), new SqlParameter("@IsDeleted", false), new SqlParameter("@type", (int)MaterialsTypeEnum.Video)).ToList();
        }

        public List<AdToCreativeBM> UpdateCreativeName(string sAdCode)
        {
            string sql = @" 
                            Select  ID into #tmp_AdToCreative
                            From    dbo.DSP_AdToCreative
                            WHERE   AdCode = @AdCode AND CHARINDEX(CONVERT(VARCHAR, ID),CreativeName) = 0 AND IsDeleted = 0 

                            UPDATE  dbo.DSP_AdToCreative SET CreativeName = CONVERT(VARCHAR, ID) + '-' + CreativeName
                            WHERE   ID IN (select ID from #tmp_AdToCreative)

                            Select * From dbo.DSP_AdToCreative WHERE   ID IN (select ID from #tmp_AdToCreative)

                            DROP TABLE #tmp_AdToCreative
                            ";

            return this.DB.Database.SqlQuery<AdToCreativeBM>(sql, new SqlParameter("@AdCode", sAdCode)).ToList();
        }

        /// <summary>
        /// 更新创意状态为草稿
        /// </summary>
        /// <param name="sAdCode"></param>
        public void UpdateCreativeToDraft(string sAdCode)
        {
            string sql = @" UPDATE dbo.DSP_AdToCreative SET State=-2 WHERE AdCode = @AdCode AND IsDeleted = 0 ";

            this.DB.Database.ExecuteSqlCommand(sql, new SqlParameter("@AdCode", sAdCode));
        }
    }
}
