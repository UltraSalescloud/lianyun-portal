using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lianyun.UST.Infrastructure.Utility
{
    public class Paging<T>
    {
        public Paging()
        { 
            DataList = null;
            PageIndex = 1;
            PageSize = 20;
            TotalCount = 0;        
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageIndex">当前页索引（0开始）</param>
        /// <param name="pageSize">页面显示最大行数</param>
        /// <param name="sortBy">排序字段</param>
        public Paging(List<T> dataList,int pageIndex, int pageSize, int totalCount)
        {
            DataList = dataList;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
        }


        public Paging(int pageIndex, int pageSize, int totalCount)
        {
            DataList = null;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public IList<T> DataList { get; set; }

        /// <summary>
        /// 当前页索引（1开始）
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页面显示最大行数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总行数(返回)
        /// </summary>
        public int TotalCount { get; set; }
    }
}