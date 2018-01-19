using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CL.Tools.Common
{
    public class LoadDll
    {
        private byte[] LoadDllFromResource(string lpFileName)
        {
            var nowAssembly = Assembly.GetEntryAssembly();
            Stream fs = null;
            try
            {
                // 尝试读取资源中的 DLL
                fs = nowAssembly.GetManifestResourceStream(nowAssembly.GetName().Name + "." + lpFileName);
            }
            finally
            {
                // 如果资源没有所需的 DLL ，就查看硬盘上有没有，有的话就读取
                if (fs == null && !File.Exists(lpFileName))
                {
                    throw (new Exception(" 找不到文件 :" + lpFileName));
                }

                if (fs == null && File.Exists(lpFileName))
                {
                    fs = new FileStream(lpFileName, FileMode.Open);
                }
                if (fs == null)
                {
                    throw new Exception("加载DLL失败");
                }
            }

            var buffer = new byte[(int)fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);
            fs.Close();
            return buffer; // 以 byte[] 返回读到的 DLL
        }

        public void Load(string[] assemblyFileNames)
        {
            if (null != assemblyFileNames && assemblyFileNames.Any())
            {
                assemblyFileNames
                    .ToList()
                    .ForEach(module => Assembly.Load(LoadDllFromResource(module)));
            }
        }

        public object Invoke(string nameSpace, string className, string lpProcName, object[] objArrayParameter)
        {
            try
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var t in assembly.GetTypes())
                    {
                        if (t.Namespace != nameSpace || t.Name != className)
                        {
                            continue;
                        }

                        var m = t.GetMethod(lpProcName);
                        if (m != null)
                        {
                            // 调用并返回
                            var obj = Activator.CreateInstance(t);
                            return m.Invoke(obj, objArrayParameter);
                        }
                    }
                }
                return null;
            }
            catch
            {
                throw new Exception("程序集未加载或调用方法失败");
            }
        }
    }
}
