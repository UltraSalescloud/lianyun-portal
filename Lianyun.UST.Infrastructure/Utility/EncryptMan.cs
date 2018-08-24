using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Lianyun.UST.Infrastructure.Utility
{
    /// <summary>
    /// 数据加密辅助类
    /// </summary>
    public class EncryptMan
    {
        #region Constructor

        /// <summary>
        /// 构造函数
        /// </summary>
        static EncryptMan()
        {
            m_RC2Key = GetBytes16(M_KEY);
            m_RC2IV = GetBytes8(M_IV);
            m_DESKey = GetBytes8(M_KEY);
            m_DESIV = GetBytes8(M_IV);
        }

        #endregion

        #region Fields

        private const string M_KEY = "Ad7/o+a-#";
        private const string M_IV = "n7&D~*H";
        private static readonly byte[] m_RC2Key;
        private static readonly byte[] m_RC2IV;
        private static readonly byte[] m_DESKey;
        private static readonly byte[] m_DESIV;
        private static ICryptoTransform m_objEncryptor;
        private static ICryptoTransform m_objDecrypor;

        #endregion

        #region Property

        /// <summary>
        /// RC2对称分组加密算法使用KEY
        /// </summary>
        public static byte[] RC2KEY
        {
            get { return m_RC2Key; }
        }

        /// <summary>
        ///  RC2对称分组加密算法初始使用矢量
        /// </summary>
        public static byte[] RC2IV
        {
            get { return m_RC2IV; }
        }

        /// <summary>
        /// DES对称加密算法使用KEY
        /// </summary>
        public static byte[] DESKey
        {
            get { return m_DESKey; }
        }

        /// <summary>
        /// DES对称加密算法初始使用矢量
        /// </summary>
        public static byte[] DESIV
        {
            get { return m_DESIV; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 对字符串进行编码转换（8byte）
        /// </summary>
        /// <param name="strDes">字符串</param>
        /// <returns>返回转换后的Bytes</returns>
        private static byte[] GetBytes8(string strDes)
        {
            int nkey = strDes.GetHashCode();
            byte[] objKey = new byte[8];
            for (int i = 0; i < objKey.Length; ++i)
            {
                objKey[i] = (byte)(((nkey << (16 - (i + 1) * 2)) | (nkey >> (i + 1) * 2)) & byte.MaxValue);
            }
            return objKey;
        }

        /// <summary>
        /// 对字符串进行编码转换（16byte）
        /// </summary>
        /// <param name="strDes">字符串</param>
        /// <returns>返回转换后的Bytes</returns>
        private static byte[] GetBytes16(string strDes)
        {
            int nkey = strDes.GetHashCode();
            byte[] objKey = new byte[16];
            for (int i = 0; i < objKey.Length; ++i)
            {
                objKey[i] = (byte)((nkey << (16 - (i + 1)) | (nkey >> (i + 1))) & byte.MaxValue);
            }
            return objKey;
        }

        /// <summary>
        /// SHA1安全哈希加密（不可逆）
        /// 
        /// 安全哈希算法（Secure Hash Algorithm）主要适用于数字签名标准（Digital Signature Standard DSS）里面定
        /// 义的数字签名算法（Digital Signature Algorithm DSA）。对于长度小于2^64位的消息，SHA1会产生一个160位
        /// 的消息摘要。当接收到消息的时候，这个消息摘要可以用来验证数据的完整性。在传输的过程中，数据很可能会
        /// 发生变化，那么这时候就会产生不同的消息摘要。 SHA1有如下特性：不可以从消息摘要中复原信息；两个不同
        /// 的消息不会产生同样的消息摘要。
        /// </summary>
        /// <param name="strSource">加密字符串</param>
        /// <returns>返回加密后的暗文</returns>
        public static string SHA1Encrypt(string strSource)
        {
            if (string.IsNullOrEmpty(strSource))
            {
                return string.Empty;
            }
            SHA1CryptoServiceProvider objSHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
            byte[] objHashCode;
            objHashCode = Encoding.UTF8.GetBytes(strSource);
            objHashCode = objSHA1CryptoServiceProvider.ComputeHash(objHashCode);
            StringBuilder sbEnText = new StringBuilder();

            foreach (byte b in objHashCode)
            {
                sbEnText.AppendFormat("{0:x2}", b);
            }
            objSHA1CryptoServiceProvider.Clear();
            return sbEnText.ToString();
        }

        /// <summary>
        /// MD5加密算法
        /// </summary>
        /// <param name="strSource">加密字符串</param>
        /// <returns>返回加密后的暗文</returns>
        public static string MD5Encrypt(string strSource)
        {
            if (string.IsNullOrEmpty(strSource))
            {
                return string.Empty;
            }
            MD5CryptoServiceProvider objMD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] objHashCode;
            objHashCode = Encoding.UTF8.GetBytes(strSource);
            objHashCode = objMD5CryptoServiceProvider.ComputeHash(objHashCode);
            StringBuilder sbEnText = new StringBuilder();

            foreach (byte b in objHashCode)
            {
                sbEnText.AppendFormat("{0:x2}", b);
            }
            objMD5CryptoServiceProvider.Clear();
            return sbEnText.ToString();
        }

        /// <summary>
        /// RC2对称分组加密
        /// 
        /// RC2是由著名密码学家Ron Rivest设计的一种传统对称分组加密算法，它可作为DES算法的建议替代算法。
        /// 它的输入和输出都是64比特。密钥的长度是从8字节到128字节可变，但目前的实现是8字节（１９９８年）。
        /// </summary>
        /// <param name="strSource">加密字符串</param>
        /// <returns>返回加密后的暗文</returns>
        public static string RC2Encrypt(string strSource)
        {
            if (string.IsNullOrEmpty(strSource))
            {
                return string.Empty;
            }
            byte[] objInputByteArray = Encoding.UTF8.GetBytes(strSource);
            RC2CryptoServiceProvider objRC2CryptoServiceProvider = new RC2CryptoServiceProvider();
            ICryptoTransform objICryptoTransform = objRC2CryptoServiceProvider.CreateEncryptor(RC2KEY, RC2IV);
            MemoryStream objMemoryStream = new MemoryStream();
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objICryptoTransform, CryptoStreamMode.Write);
            objCryptoStream.Write(objInputByteArray, 0, objInputByteArray.Length);
            objCryptoStream.FlushFinalBlock();
            objRC2CryptoServiceProvider.Clear();
            return Convert.ToBase64String(objMemoryStream.ToArray());
        }

        /// <summary>
        /// RC2RC2对称分组解密
        /// </summary>
        /// <param name="strDes">解密字符串</param>
        /// <returns>返回解密后的明文</returns>
        public static string RC2Decrypt(string strDes)
        {
            if (string.IsNullOrEmpty(strDes))
            {
                return string.Empty;
            }
            byte[] objInputByteArray = Convert.FromBase64String(strDes);
            RC2CryptoServiceProvider objRC2CryptoServiceProvider = new RC2CryptoServiceProvider();
            ICryptoTransform objICryptoTransform = objRC2CryptoServiceProvider.CreateDecryptor(RC2KEY, RC2IV);
            MemoryStream objMemoryStream = new MemoryStream();
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objICryptoTransform, CryptoStreamMode.Write);
            objCryptoStream.Write(objInputByteArray, 0, objInputByteArray.Length);
            objCryptoStream.FlushFinalBlock();
            objRC2CryptoServiceProvider.Clear();
            return Encoding.UTF8.GetString(objMemoryStream.ToArray());
        }

        /// <summary>
        /// DES对称加密
        /// </summary>
        /// <param name="strDes">加密字符串</param>
        /// <returns>返回加密后的暗文</returns>
        public static string DESEncrypt(string strSource)
        {
            #region --2012-9-4--
            //if (string.IsNullOrEmpty(strSource))
            //{
            //    return string.Empty;
            //}
            //byte[] objInputByteArray = Encoding.UTF8.GetBytes(strSource);
            //DESCryptoServiceProvider objDESCryptoServiceProvider = new DESCryptoServiceProvider();
            //ICryptoTransform objICryptoTransform = objDESCryptoServiceProvider.CreateEncryptor(DESKey, DESIV);
            //MemoryStream objMemoryStream = new MemoryStream();
            //CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objICryptoTransform, CryptoStreamMode.Write);
            //objCryptoStream.Write(objInputByteArray, 0, objInputByteArray.Length);
            //objCryptoStream.FlushFinalBlock();
            //objDESCryptoServiceProvider.Clear();
            //return Convert.ToBase64String(objMemoryStream.ToArray());
            #endregion

            return EncryptHelper.Encrypt(strSource, M_KEY);
        }

        /// <summary>
        /// DES对称加密
        /// </summary>
        /// <param name="strSource">加密字符串</param>
        /// <param name="key">密钥KEY</param>
        /// <returns>返回加密后的暗文</returns>
        public static string DESEncrypt(string strSource, string key)
        {
            #region --2012-9-4--
            //if (string.IsNullOrEmpty(strSource))
            //{
            //    return string.Empty;
            //}
            //byte[] objInputByteArray = Encoding.UTF8.GetBytes(strSource);
            //DESCryptoServiceProvider objDESCryptoServiceProvider = new DESCryptoServiceProvider();
            //byte[] objKey = Encoding.UTF8.GetBytes(key);
            //ICryptoTransform Encryptor = objDESCryptoServiceProvider.CreateEncryptor(objKey, DESIV);
            //MemoryStream objMemoryStream = new MemoryStream();
            //CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, Encryptor, CryptoStreamMode.Write);
            //objCryptoStream.Write(objInputByteArray, 0, objInputByteArray.Length);
            //objCryptoStream.FlushFinalBlock();
            //objDESCryptoServiceProvider.Clear();
            //return Convert.ToBase64String(objMemoryStream.ToArray());
            #endregion

            return EncryptHelper.Encrypt(strSource, M_KEY);
        }

        /// <summary>
        /// DES对称解密
        /// </summary>
        /// <param name="strDes">解密字符串</param>
        /// <returns>返回解密后的明文</returns>
        public static string DESDecrypt(string strDes)
        {
            #region --2012-9-4--
            //if (string.IsNullOrEmpty(strDes))
            //{
            //    return string.Empty;
            //}
            //byte[] objInputByteArray = Convert.FromBase64String(strDes);
            //DESCryptoServiceProvider objDESCryptoServiceProvider = new DESCryptoServiceProvider();
            //ICryptoTransform objICryptoTransform = objDESCryptoServiceProvider.CreateDecryptor(DESKey, DESIV);
            //MemoryStream objMemoryStream = new MemoryStream();
            //CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objICryptoTransform, CryptoStreamMode.Write);
            //objCryptoStream.Write(objInputByteArray, 0, objInputByteArray.Length);
            //objCryptoStream.FlushFinalBlock();
            //objDESCryptoServiceProvider.Clear();
            //return Encoding.UTF8.GetString(objMemoryStream.ToArray());
            #endregion

            return EncryptHelper.Descrypt(strDes, M_KEY);
        }

        /// <summary>
        /// 对称解密
        /// </summary>
        /// <param name="strDes">解密字符串</param>
        /// <param name="key">密钥KEY</param>
        /// <returns>返回解密后的明文</returns>
        public static string DESDecrypt(string strDes, string key)
        {
            #region --2012-9-4--
            //if (string.IsNullOrEmpty(strDes))
            //{
            //    return string.Empty;
            //}
            //byte[] objInputByteArray = Convert.FromBase64String(strDes);
            //byte[] objKey = Encoding.UTF8.GetBytes(key);
            //DESCryptoServiceProvider objDESCryptoServiceProvider = new DESCryptoServiceProvider();
            //ICryptoTransform objICryptoTransform = objDESCryptoServiceProvider.CreateDecryptor(objKey, DESIV);
            //MemoryStream objMemoryStream = new MemoryStream();
            //CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objICryptoTransform, CryptoStreamMode.Write);
            //objCryptoStream.Write(objInputByteArray, 0, objInputByteArray.Length);
            //objCryptoStream.FlushFinalBlock();
            //objDESCryptoServiceProvider.Clear();
            //return Encoding.UTF8.GetString(objMemoryStream.ToArray());
            #endregion

            return EncryptHelper.Descrypt(strDes, M_KEY);
        }

        #endregion
    }

    /// <summary>
    /// 数据加密扩展类
    /// </summary>
    public static class EncryptHelper
    {
        /// <summary>MD5散列</summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String Hash(String str)
        {
            if (String.IsNullOrEmpty(str)) return null;

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            Byte[] by = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            return BitConverter.ToString(by).Replace("-", "");
        }

        #region 加解密
        private static SymmetricAlgorithm GetProvider(String key)
        {
            SymmetricAlgorithm sa = new TripleDESCryptoServiceProvider();
            Int32 max = sa.LegalKeySizes[0].MaxSize / 8;
            key = Hash(key);
            String str = key;
            Byte[] bts = Encoding.UTF8.GetBytes(str);

            if (bts.Length != max) Array.Resize<Byte>(ref bts, max);

            sa.Key = bts;

            max = sa.LegalBlockSizes[0].MaxSize / 8;
            bts = Encoding.UTF8.GetBytes(str);
            //倒序
            Array.Reverse(bts);
            if (bts.Length != max) Array.Resize<Byte>(ref bts, max);
            sa.IV = bts;

            return sa;
        }

        /// <summary>TripleDES加密</summary>
        /// <param name="content">UTD8编码的明文</param>
        /// <param name="key">密码字符串经MD5散列后作为DES密码</param>
        /// <returns></returns>
        public static String Encrypt(String content, String key)
        {
            if (String.IsNullOrEmpty(content)) throw new ArgumentNullException("content");
            Byte[] data = Encoding.UTF8.GetBytes(content);

            data = Encrypt(data, key);

            return Convert.ToBase64String(data);
        }

        /// <summary>TripleDES加密</summary>
        /// <param name="data"></param>
        /// <param name="key">密码字符串经MD5散列后作为DES密码</param>
        /// <returns></returns>
        public static Byte[] Encrypt(Byte[] data, String key)
        {
            if (data == null || data.Length < 1) throw new ArgumentNullException("data");
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            MemoryStream outstream = new MemoryStream();
            CryptoStream stream = new CryptoStream(outstream, GetProvider(key).CreateEncryptor(), CryptoStreamMode.Write);
            stream.Write(data, 0, data.Length);
            stream.FlushFinalBlock();

            data = outstream.ToArray();

            stream.Close();
            outstream.Close();

            return data;
        }

        /// <summary>TripleDES解密</summary>
        /// <param name="content">UTD8编码的密文</param>
        /// <param name="key">密码字符串经MD5散列后作为DES密码</param>
        /// <returns></returns>
        public static String Descrypt(String content, String key)
        {
            if (String.IsNullOrEmpty(content)) throw new ArgumentNullException("content");
            Byte[] data = Convert.FromBase64String(content);

            data = Descrypt(data, key);

            return Encoding.UTF8.GetString(data);
        }

        /// <summary>TripleDES解密</summary>
        /// <param name="data"></param>
        /// <param name="key">密码字符串经MD5散列后作为DES密码</param>
        /// <returns></returns>
        public static Byte[] Descrypt(Byte[] data, String key)
        {
            if (data == null || data.Length < 1) throw new ArgumentNullException("data");
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            MemoryStream ms = new MemoryStream(data);
            CryptoStream stream = new CryptoStream(ms, GetProvider(key).CreateDecryptor(), CryptoStreamMode.Read);

            MemoryStream ms2 = new MemoryStream();
            while (true)
            {
                Byte[] buffer = new Byte[10];
                Int32 count = stream.Read(buffer, 0, buffer.Length);
                if (count <= 0) break;

                ms2.Write(buffer, 0, count);
                if (count < buffer.Length) break;
            }

            data = ms2.ToArray();

            stream.Close();
            ms.Close();
            ms2.Close();

            return data;
        }
        #endregion

        #region RC4加密
        /// <summary>RC4加密解密</summary>
        /// <param name="data">数据</param>
        /// <param name="pass">UTF8编码的密码</param>
        /// <returns></returns>
        public static Byte[] RC4(Byte[] data, String pass)
        {
            if (data == null || pass == null) return null;
            Byte[] output = new Byte[data.Length];
            Int64 i = 0;
            Int64 j = 0;
            Byte[] mBox = GetKey(Encoding.UTF8.GetBytes(pass), 256);

            // 加密
            for (Int64 offset = 0; offset < data.Length; offset++)
            {
                i = (i + 1) % mBox.Length;
                j = (j + mBox[i]) % mBox.Length;
                Byte temp = mBox[i];
                mBox[i] = mBox[j];
                mBox[j] = temp;
                Byte a = data[offset];
                //Byte b = mBox[(mBox[i] + mBox[j] % mBox.Length) % mBox.Length];
                // mBox[j] 一定比 mBox.Length 小，不需要在取模
                Byte b = mBox[(mBox[i] + mBox[j]) % mBox.Length];
                output[offset] = (Byte)((Int32)a ^ (Int32)b);
            }

            return output;
        }

        /// <summary>打乱密码</summary>
        /// <param name="pass">密码</param>
        /// <param name="kLen">密码箱长度</param>
        /// <returns>打乱后的密码</returns>
        static Byte[] GetKey(Byte[] pass, Int32 kLen)
        {
            Byte[] mBox = new Byte[kLen];

            for (Int64 i = 0; i < kLen; i++)
            {
                mBox[i] = (Byte)i;
            }
            Int64 j = 0;
            for (Int64 i = 0; i < kLen; i++)
            {
                j = (j + mBox[i] + pass[i % pass.Length]) % kLen;
                Byte temp = mBox[i];
                mBox[i] = mBox[j];
                mBox[j] = temp;
            }
            return mBox;
        }
        #endregion

        #region RSA签名
        /// <summary>签名</summary>
        /// <param name="data"></param>
        /// <param name="priKey"></param>
        /// <returns>Base64编码的签名</returns>
        internal static String Sign(Byte[] data, String priKey)
        {
            if (data == null | String.IsNullOrEmpty(priKey)) return null;

            var rsa = new RSACryptoServiceProvider();
            var md5 = new MD5CryptoServiceProvider();
            try
            {
                rsa.FromXmlString(priKey);
                return Convert.ToBase64String(rsa.SignHash(md5.ComputeHash(data), "1.2.840.113549.2.5"));
            }
            catch { return null; }
        }
        #endregion

        #region RSA验证签名
        /// <summary>验证签名</summary>
        /// <param name="data">待验证的数据</param>
        /// <param name="signdata">Base64编码的签名</param>
        /// <param name="pubKey">公钥</param>
        /// <returns></returns>
        internal static Boolean Verify(Byte[] data, String signdata, String pubKey)
        {
            if (data == null ||
                data.Length < 1 ||
                String.IsNullOrEmpty(signdata) ||
                String.IsNullOrEmpty(pubKey)) return false;

            var rsa = new RSACryptoServiceProvider();
            var md5 = new MD5CryptoServiceProvider();
            try
            {
                rsa.FromXmlString(pubKey);
                return rsa.VerifyHash(md5.ComputeHash(data), "1.2.840.113549.2.5", Convert.FromBase64String(signdata));
            }
            catch { return false; }
        }
        #endregion

        #region 编码
        /// <summary>把字节数组编码为十六进制字符串</summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static String ToHex(this Byte[] data, Int32 offset = 0, Int32 count = 0)
        {
            if (data == null || data.Length < 1) return null;
            if (count <= 0) count = data.Length - offset;

            Char[] cs = new Char[count * 2];
            // 两个索引一起用，避免乘除带来的性能损耗
            for (int i = 0, j = 0; i < count; i++, j += 2)
            {
                Byte b = data[offset + i];
                cs[j] = GetHexValue(b / 0x10);
                cs[j + 1] = GetHexValue(b % 0x10);
            }
            return new String(cs);
        }

        private static char GetHexValue(int i)
        {
            if (i < 10) return (char)(i + 0x30);
            return (char)(i - 10 + 0x41);
        }

        /// <summary>解密</summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Byte[] FromHex(String data)
        {
            if (String.IsNullOrEmpty(data)) return null;

            Byte[] bts = new Byte[data.Length / 2];
            for (int i = 0; i < data.Length / 2; i++)
            {
                bts[i] = (Byte)Convert.ToInt32(data.Substring(2 * i, 2), 16);
            }
            return bts;
        }
        #endregion
    }
}
