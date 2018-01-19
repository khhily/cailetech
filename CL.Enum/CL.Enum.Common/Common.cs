using System.ComponentModel;
using System.Reflection;

namespace CL.Enum.Common
{
    public static class Common
    {
        /// <summary>
		/// 从枚举中获取Description
		/// </summary>
		/// <param name="enumName">需要获取枚举描述的枚举</param>
		/// <returns>描述内容</returns>
		public static string GetDescription(this System.Enum enumName)
        {
            string _description = string.Empty;
            FieldInfo _fieldInfo = enumName.GetType().GetField(enumName.ToString());
            DescriptionAttribute[] _attributes = _fieldInfo.GetDescriptAttr();
            if (_attributes != null && _attributes.Length > 0)
                _description = _attributes[0].Description;
            else
                _description = enumName.ToString();
            return _description;
        }
        public static DescriptionAttribute[] GetDescriptAttr(this FieldInfo fieldInfo)
        {
            if (fieldInfo != null)
            {
                return (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            }
            return null;
        }
    }
}
