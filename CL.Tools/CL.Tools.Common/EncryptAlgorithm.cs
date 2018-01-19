using System;
using System.Security.Cryptography;
using System.Text;

namespace CL.Tools.Common
{
    public class EncryptAlgorithm
    {
        /// <summary>
        /// 获取一个标准MD5 32位加密值
        /// </summary>
        /// <param name="strSource">待加密参数</param>
        public static string MD5(string strSource)
        {
            //微软md5方法参考return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5");
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(strSource));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// 获取一个变异MD5 32位加密值(用于私有系统)
        /// </summary>
        /// <param name="strSource">待加密参数</param>
        public static string MD5ForVariation(string strSource)
        {
            return MD5("sa.fe" + strSource + "_mdcaile5");
        }

        /// <summary>
        /// 获取一个标准SHA1加密值
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string SHA1(string strSource)
        {
            SHA1 shaTmp = new SHA1CryptoServiceProvider();

            //获取密文字节数组
            byte[] bytResult = shaTmp.ComputeHash(Encoding.UTF8.GetBytes(strSource));

            //转换成字符串，并取9到25位
            //string strResult = BitConverter.ToString(bytResult, 4, 8);
            //转换成字符串，32位

            //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉
            return BitConverter.ToString(bytResult).Replace("-", "");
        }

        /// <summary>
        /// 获取一个变异SHA1 加密值(用于私有系统)
        /// </summary>
        /// <param name="strSource">待加密参数</param>
        public static string SHA1ForVariation(string strSource)
        {
            return SHA1("ca.fe" + strSource + "_shcailae1").ToLower();
        }

        /// <summary>
        /// 256位AES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <param name="bKey">UTF8.GetBytes 后的KEY</param>
        /// <param name="bIV">UTF8.GetBytes 后的IV</param>
        /// <returns>密文</returns>
        public static string AESEncrypt(string plainStr, byte[] bKey, byte[] bIV)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = bKey;
            rDel.IV = bIV;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(byteArray, 0, byteArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 256位AES解密
        /// </summary>
        /// <param name="encryptStr">密文字符串</param>
        /// <param name="bKey">UTF8.GetBytes 后的KEY</param>
        /// <param name="bIV">UTF8.GetBytes 后的IV</param>
        /// <returns>明文</returns>
        public static string AESDecrypt(string encryptStr, byte[] bKey, byte[] bIV)
        {
            byte[] byteArray = Convert.FromBase64String(encryptStr);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = bKey;
            rDel.IV = bIV;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(byteArray, 0, byteArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// 生成彩券加密Key(字符长度12-16之间)
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string CustomEncryptKey(string strSource)
        {
            string rec = string.Empty;
            int length = 2;
            int strLen = strSource.Length / 2;
            string interim = string.Empty;
            bool isrem = false;
            if ((strSource.Length % 2) > 0)
            {
                isrem = true;
                strLen += 1;
            }
            for (int i = 0; i < strLen; i++)
            {
                if (i == strLen - 1)
                    if (isrem)
                        interim = strSource.Substring(i * length, 1);
                    else
                        interim = strSource.Substring(i * length, length);
                else
                    interim = strSource.Substring(i * length, length);
                rec += MD5(interim).Substring(0, 2);
            }
            return rec.ToUpper();
        }
    }
}
