using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Infrastructure.Page
{
    /// <summary>
    /// 分页辅助类
    /// 创建人：朱斌
    /// 创建时间：2015-04-22
    /// </summary>
    public class BuildPagerSqlHelper
    {
        /// <summary>
        /// 生成分页TSQL
        /// </summary>
        /// <param name="nPageSize">页容量</param>
        /// <param name="nGotoPage">第几页</param>
        /// <param name="sSelect">查询的字段(*代表所有)</param>
        /// <param name="sFrom">数据源sql语句</param>
        /// <param name="sOrder">排序字符串</param>
        /// <returns></returns>
        public static string BuildPagerDateSetSql(int nPageSize, int nGotoPage, string sSelect, string sFrom, string sOrder)
        {
            string sTmp = "";
            string sTopShow = "";
            if (nGotoPage == 1)
            {
                sTopShow = "top " + nPageSize + "  ";
            }
            if (string.IsNullOrEmpty(sSelect))
            {
                sSelect = "*";
            }
            sTmp = @"declare @nPageSize int, @nGotoPage int;
                    set @nPageSize  = {0};
                    set @nGotoPage = {1};

                    select 
                            * 
                        from
                            (
                                select 
                                        {4}
                                        Row_Number() over ({5}) as _RowID, 
                                    {2}
                                    from
                                    {3}
                            ) as T
                        where 
                            T._RowID > ((@nGotoPage - 1) * @nPageSize)
                            and T._RowID <= (@nGotoPage * @nPageSize) ";

            sTmp = string.Format(sTmp, nPageSize, nGotoPage, sSelect, sFrom, sTopShow, " order by " + sOrder);
            return sTmp;
        }
    }
}
