using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Infrastructure.Utility
{
    /// <summary>
    /// 导出帮助类
    /// 尹洲
    /// 20150413
    /// </summary>
    public class Export
    {
        //public ActionResult ExportCSV(DataTable dt, string filename, string header = "", string footer = "")
        //{return File(outBuffer, "application/ms-excel");}
        public byte[] ExportCSV(DataTable dt, string filename, string header = "", string footer = "")
        {
            //string attachment = "attachment; filename=" + HttpUtility.UrlEncode(filename, Encoding.UTF8) + ".csv";
            string attachment = "attachment; filename=" + filename + ".csv";
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            System.Web.HttpContext.Current.Response.ContentType = "text/csv";
            System.Web.HttpContext.Current.Response.AddHeader("Pragma", "public");

            StringBuilder str = new StringBuilder();

            if (!string.IsNullOrEmpty(header))
            {
                str.Append(header);
                str.Append(Environment.NewLine);
                str.Append(Environment.NewLine);
            }


            for (int i = 0; i < dt.Columns.Count; i++)
            {
                str.AppendFormat("{0},", dt.Columns[i].ColumnName);
            }
            str.Append(Environment.NewLine);
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    str.AppendFormat("{0},", string.IsNullOrEmpty(dr[i].ToString()) ? "0" : dr[i].ToString());
                }
                str.Append(Environment.NewLine);
            }


            if (!string.IsNullOrEmpty(footer))
            {
                //str.Append(Environment.NewLine);
                str.Append(footer);
                str.Append(Environment.NewLine);
            }
            byte[] strByt = System.Text.Encoding.UTF8.GetBytes(str.ToString());

            byte[] outBuffer = new byte[strByt.Length + 3];
            //有BOM,解决乱码
            outBuffer[0] = (byte)0xEF;
            outBuffer[1] = (byte)0xBB;
            outBuffer[2] = (byte)0xBF;
            Array.Copy(strByt, 0, outBuffer, 3, strByt.Length);

            return outBuffer;
           // return File(outBuffer, "application/ms-excel");
        }

        /// <summary>
        /// 创建Table(根据传入的列名)
        /// </summary>
        /// <param name="Names"></param>
        /// <returns></returns>
        public DataTable CreateTable(string[] Names)
        {
            DataTable dt = new DataTable();
            foreach (var item in Names)
            {
                dt.Columns.Add(CreateColumn(item));
            }

            return dt;
        }
        private DataColumn CreateColumn(string ColumnName)
        {
            DataColumn column = new DataColumn();
            column.ColumnName = ColumnName;
            return column;
        }
        /// <summary>
        /// 循环去除datatable中的空行
        /// </summary>
        /// <param name="dt"></param>

        protected DataTable RemoveEmpty(DataTable dt)
        {
            List<DataRow> removelist = new List<DataRow>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool rowdataisnull = true;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
                    {
                        rowdataisnull = false;
                    }

                }
                if (rowdataisnull)
                {
                    removelist.Add(dt.Rows[i]);
                }

            }
            for (int i = 0; i < removelist.Count; i++)
            {
                dt.Rows.Remove(removelist[i]);
            }
            return dt;
        }

        /// <summary>
        /// 00转成00:00时间格式
        /// 尹洲
        /// 20150413
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public string FomartHour(string hour)
        {
            hour = hour + ":00";
            DateTime dt = DateTime.ParseExact(hour, "HH:mm", null);
            DateTime dt2 = dt.AddMinutes(59);
            string time = dt.ToString("HH:mm") + "-" + dt2.ToString("HH:mm");
            return time;
        }


        /// <summary>
        /// 格式化数字字符串成xxx万--数量
        /// </summary>
        /// <param name="num"></param>
        /// <param name="minNum"></param>
        /// <param name="BaseNum"></param>
        /// <returns></returns>
        public string FormatNum(long num, long minNum = 1000000, decimal BaseNum = 10000)
        {
            string retStr = string.Empty;
            if (BaseNum <= 0)
            {
                return retStr;
            }

            if (num < minNum)
            {
                retStr = num.ToString("N0");
            }
            else
            {
                decimal showNum = Math.Floor(num / BaseNum);
                retStr = showNum.ToString("N0") + "w";
            }
            return retStr;
        }


        /// <summary>
        /// 格式化字符串成xxx万---金额
        /// </summary>
        /// <param name="strnum"></param>
        /// <param name="minNum"></param>
        /// <param name="BaseNum"></param>
        /// <returns></returns>
        public string FormatNum(string strnum, bool needRound = false, long minNum = 1000000, decimal BaseNum = 10000, int keep = 2)
        {
            string retStr = string.Empty;
            long num = 0;

            //小数
            if (strnum.Contains("."))
            {
                string newStrNum = strnum.Split('.')[0];
                if (!long.TryParse(newStrNum, out num))
                {
                    return retStr;
                }
                else
                {
                    if (num < minNum)
                    {
                        decimal dNum = 0;
                        decimal.TryParse(strnum, out dNum);
                        if (needRound)
                        {
                            return string.Format("{0:N" + keep.ToString() + "}", Math.Round(dNum, keep, MidpointRounding.AwayFromZero));

                        }
                        else
                        {
                            return string.Format("{0:N}", dNum);
                        }
                    }
                    else
                    {
                        decimal showNum = Math.Floor(num / BaseNum);
                        return showNum.ToString("N0") + "w";
                    }
                }
            }

            //非小数
            if (!long.TryParse(strnum, out num))
            {
                return retStr;
            }

            if (BaseNum <= 0)
            {
                return retStr;
            }

            if (num < minNum)
            {
                retStr = num.ToString("N0");
            }
            else
            {
                decimal showNum = Math.Floor(num / BaseNum);
                retStr = showNum.ToString("N0") + "w";
            }
            return retStr;
        }


    }
}
