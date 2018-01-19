using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Tools.Common
{
    public static class RequestInputStream
    {
        /// <summary>
        /// Post数据解析
        /// </summary>
        /// <param name="InputStream"></param>
        /// <returns></returns>
        public static Dictionary<string, string> SetParameters(Stream InputStream)
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                StreamReader Reader = new StreamReader(InputStream);
                string Obj_Parameters = Reader.ReadLine();
                if (!string.IsNullOrEmpty(Obj_Parameters))
                {
                    string[] Parameters = Obj_Parameters.Split('&');
                    foreach (string Parameter in Parameters)
                    {
                        if (!string.IsNullOrEmpty(Parameter.Trim()))
                        {
                            string[] val = Parameter.Split('=');
                            if (!dic.ContainsKey(val[0].Trim()))
                                dic.Add(val[0], val[1]);
                        }
                    }
                }
                return dic;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("POST参数解析错误[SetParameters]：{0}", ex.Message));
            }
        }

        public static Dictionary<string, object> SetParameters(string request, ref long UnixTime, ref string Digest)
        {
            string[] nameValues = request.Split('&');
            Dictionary<string,object> parameters = new Dictionary<string, object>();
            foreach (string value in nameValues)
            {
                string[] item = value.Split('=');
                if (item[0].ToLower() != "unixtime" && item[0].ToLower() != "digest")
                    parameters.Add(item[0], item[1]);
                else
                {
                    if (item[0].ToLower() == "unixtime")
                        UnixTime = Convert.ToInt64(item[1]);
                    else if (item[0].ToLower() == "digest")
                        Digest = Convert.ToString(item[1]);
                }
            }
            return parameters;
        }
        public static Dictionary<string, object> SetParameters(Stream InputStream, ref long UnixTime, ref string Digest)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string Obj_Parameters = new StreamReader(InputStream).ReadLine();
            if (!string.IsNullOrEmpty(Obj_Parameters.Trim()))
            {
                string[] Parameters = Obj_Parameters.Split('&');
                foreach (string Parameter in Parameters)
                {
                    if (!string.IsNullOrEmpty(Parameter.Trim()))
                    {
                        string[] item = Parameter.Split('=');
                        if (item[0].ToLower() != "unixtime" && item[0].ToLower() != "digest")
                            parameters.Add(item[0], item[1]);
                        else
                        {
                            if (item[0].ToLower() == "unixtime")
                                UnixTime = Convert.ToInt64(item[1]);
                            else if (item[0].ToLower() == "digest")
                                Digest = Convert.ToString(item[1]);
                        }
                    }
                }
            }
            return parameters;
        }
    }
}
