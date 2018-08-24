
using System.Text.RegularExpressions;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Lianyun.UST.Infrastructure
{
    public static class Extensions
    {
        private static Regex emailRegex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        private static Regex int32Regex = new Regex(@"^[-]?[0-9]*[.]?[0-9]*$");
        private static Regex doubleRegex = new Regex(@"^([0-9])[0-9]*(\.\w*)?$");

        public static string Encrypt(this string s, string key)
        {
            return Cryptography.Encrypt(s, key);
        }

        public static string Decrypt(this string s, string key)
        {
            return Cryptography.Decrypt(s, key);
        }

        public static bool IsNotEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static bool IsEmail(this string value)
        {
            bool result = false;
            if (value.IsNotEmpty())
            {
                result = emailRegex.IsMatch(value);
            }
            return result;
        }

        public static bool IsNumeric(this string value)
        { 
            bool result=false;
            if (value.IsNotEmpty())
            {
                if (value.Length > 0 && value.Length <= 11 && int32Regex.IsMatch(value))
                {
                    if ((value.Length < 10) || (value.Length == 10 && value[0] == '1') || (value.Length == 11 && value[0] == '-' && value[1] == '1'))
                        result = true;
                }
            }

            return result;
        }

        public static bool IsNumericArray(this List<string> list)
        {
            bool result = false;
            if (list.IsNotNull())
            {
                foreach (string value in list)
                {
                    if (!value.IsNumeric())
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        public static bool IsDouble(this string value)
        {
            bool result = false;
            if (value.IsNotEmpty())
            {
                result = doubleRegex.IsMatch(value);
            }
            return result;
        }

        public static bool IsDefaultValue(this int value)
        {
            return value == default(int) ? true : false;
        }

        public static bool IsDefaultValue(this long value)
        {
            return value == default(long) ? true : false;
        }

        public static bool IsNotNull(this IList list)
        {
            return list != null && list.Count > 0;
        }

        public static bool SafeBoolParse(this string value)
        {
            bool result = false;
            if (value.IsNotEmpty())
            {
                bool.TryParse(value, out result);
            }
            return result;
        }

        public static int SafeIntParse(this string value)
        {
            int nResult = 0;
            if (value.IsNotEmpty())
            {
                Int32.TryParse(value, out nResult);
            }

            return nResult;
        }

        public static int SafeIntParse(this string value,int nDefault)
        {
            int nResult = nDefault;
            if (value.IsNotEmpty())
            {
                Int32.TryParse(value, out nResult);
            }

            return nResult;
        }

        public static DateTime SafeDateTimeParse(this string value)
        {
            DateTime dtResult = DateTime.MinValue;

            if(value.IsNotEmpty())
            {
                DateTime.TryParse(value,out dtResult);
            }

            return dtResult;
        }

        public static decimal SafeDecimalParse(this string value)
        {
            decimal mResult = 0.0m;

            if (value.IsNotEmpty())
            {
                Decimal.TryParse(value, out mResult);
            }

            return mResult;
        }

        public static long SafeLongParse(this string value)
        {
            long lResult = 0;

            if (value.IsNotEmpty())
            {
                long.TryParse(value, out lResult);
            }

            return lResult;
        }

        public static double SafeDoubleParse(this string value)
        {
            double dResult = 0;

            if (value.IsNotEmpty())
            {
                double.TryParse(value,out dResult);
            }

            return dResult;
        }

        public static string ConvertToEncodeUrl(this string str)
        {
            return System.Web.HttpUtility.UrlEncode(str);
        }
        public static string ConvertoDecodeUrl(this string str)
        {
            return System.Web.HttpUtility.UrlDecode(str);
        }
    }
}
