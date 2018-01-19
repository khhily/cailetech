using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CL.Tools.Common
{
    public class GenericRegulate<T>
    {
        /// <summary>
        /// 设置
        /// 格式：pk:1,min:2,max:3,award:4|pk:11,min:22,max:33,award:44
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="temp"></param>
        /// <returns></returns>
        public List<T> SetString(string param)
        {
            try
            {
                List<T> arrayList = new List<T>();
                if (string.IsNullOrEmpty(param.Trim()))
                    arrayList = null;
                else
                {
                    string[] array = param.Split('|');
                    foreach (string item in array)
                    {
                        string[] entitys = item.Split(',');
                        T temp = (T)Activator.CreateInstance(typeof(T));
                        foreach (string entity in entitys)
                        {
                            string[] fieldObj = entity.Split(':');
                            if (fieldObj.Length == 2)
                            {
                                PropertyInfo[] PropertyInfo = temp.GetType().GetProperties();
                                foreach (PropertyInfo Property in PropertyInfo)
                                    if (Property.Name.Trim() == fieldObj[0].Trim())
                                    {
                                        switch (Property.PropertyType.FullName)
                                        {
                                            case "System.Int16":
                                                Property.SetValue(temp, Convert.ToInt16(fieldObj[1]));
                                                break;
                                            case "System.Int32":
                                                Property.SetValue(temp, Convert.ToInt32(fieldObj[1]));
                                                break;
                                            case "System.Int64":
                                                long val = Convert.ToInt64(fieldObj[1]);
                                                if (Property.Name.Trim() == "award" || Property.Name.Trim() == "min" | Property.Name.Trim() == "max")
                                                    val = val * 100;
                                                Property.SetValue(temp, val);
                                                break;

                                        }
                                        continue;
                                    }
                            }
                        }
                        arrayList.Add(temp);
                    }
                }
                return arrayList;
            }
            catch
            {
                throw;
            }
        }
    }
}
