using System;
using System.IO;
using System.Xml.Serialization;

namespace CL.Tools.Common
{
    public class XmlHelper
    {
        #region 数据截取

        /// <summary>
        /// 获取xml中指定元素内部分
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string GetXmlElement(string xml, string element = "body")
        {
            if (xml.Contains("<" + element + ">") && xml.Contains("</" + element + ">"))
            {
                string xmlNew = xml.Replace("<" + element + ">", "@").Replace("</" + element + ">", "@");
                string[] arr = xmlNew.Split('@');
                if (arr.Length >= 3)
                {
                    return arr[1];
                }
                return "1";
            }
            return "-1";
        }
        #endregion

        #region 反序列化 
        /// <summary> 
        /// 反序列化 
        /// </summary> 
        /// <param name="type">类型</param> 
        /// <param name="xml">XML字符串</param> 
        /// <returns></returns> 
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary> 
        /// 反序列化 
        /// </summary> 
        /// <param name="type"></param> 
        /// <param name="xml"></param> 
        /// <returns></returns> 
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region 序列化 
        /// <summary> 
        /// 序列化 
        /// </summary> 
        /// <param name="type">类型</param> 
        /// <param name="obj">对象</param> 
        /// <returns></returns> 
        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象 
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

        #endregion

    }
}
