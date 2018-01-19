using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CL.Tools.Common
{
    public class IniFile
    {
        private object m_lock = new object();
        private string m_FileName = null;

        /// <summary>
        /// Ini文件名
        /// </summary>
        public string FileName
        {
            get { return m_FileName; }
        }

        private bool m_Lazy = false;
        private Dictionary<string, Dictionary<string, string>> m_Sections = new Dictionary<string, Dictionary<string, string>>();
        private bool m_CacheModified = false;

        public IniFile(string FileName)
        {
            Initialize(FileName, false);
        }

        public IniFile(string FileName, bool Lazy)
        {
            Initialize(FileName, Lazy);
        }

        public void Initialize(string FileName, bool Lay)
        {
            m_FileName = FileName;
            m_Lazy = Lay;
            if (!m_Lazy) Refresh();
        }

        private void Refresh()
        {
            lock (m_lock)
            {
                StreamReader sr = null;
                try
                {
                    m_Sections.Clear();
                    try
                    {
                        sr = new StreamReader(m_FileName);
                    }
                    catch (FileNotFoundException) { return; }

                    Dictionary<string, string> CurrentSection = null;
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        s = s.Trim();
                        if (s.StartsWith("#"))
                            continue;
                        if (s.StartsWith("[") && s.EndsWith("]"))
                        {
                            if (s.Length > 2)
                            {
                                string SectionName = s.Substring(1, s.Length - 2);
                                if (m_Sections.ContainsKey(SectionName))
                                    CurrentSection = null;
                                else
                                {
                                    CurrentSection = new Dictionary<string, string>();
                                    m_Sections.Add(SectionName, CurrentSection);
                                }
                            }
                        }
                        else if (CurrentSection != null)
                        {
                            int i;
                            if ((i = s.IndexOf("=")) > 0)
                            {
                                int j = s.Length - i - 1;
                                string Key = s.Substring(0, i).Trim();
                                if (Key.Length > 0)
                                {
                                    if (!CurrentSection.ContainsKey(Key))
                                    {
                                        string Value = (j > 0) ? (s.Substring(i + 1, j).Trim()) : "";
                                        CurrentSection.Add(Key, Value);
                                    }
                                }
                            }
                        }
                    }
                }
                finally
                {
                    if (sr != null) sr.Close();
                    sr = null;
                }
            }
        }

        private void Flush()
        {
            lock (m_lock)
            {
                if (!m_CacheModified) return;
                m_CacheModified = false;

                StreamWriter sw = new StreamWriter(m_FileName);
                try
                {
                    bool First = false;
                    foreach (KeyValuePair<string, Dictionary<string, string>> SectionPair in m_Sections)
                    {
                        Dictionary<string, string> Section = SectionPair.Value;
                        if (First) sw.WriteLine();
                        First = true;

                        sw.Write('[');
                        sw.Write(SectionPair.Key);
                        sw.WriteLine(']');

                        foreach (KeyValuePair<string, string> ValuePair in Section)
                        {
                            sw.Write(ValuePair.Key);
                            sw.Write('=');
                            sw.WriteLine(ValuePair.Value);
                        }
                    }
                }
                finally
                {
                    if (sw != null) sw.Close();
                    sw = null;
                }
            }
        }

        public string Read(string SectionName, string Key)
        {
            if (m_Lazy)
            {
                m_Lazy = false;
                Refresh();
            }
            lock (m_lock)
            {
                Dictionary<string, string> Section;
                if (!m_Sections.TryGetValue(SectionName, out Section)) return "";
                string Value;
                if (!Section.TryGetValue(Key, out Value)) return "";
                return Value;
            }
        }

        private bool Write(string SectionName, string Key, object Value)
        {
            if (m_Lazy)
            {
                m_Lazy = false;
                Refresh();
            }

            lock (m_lock)
            {
                m_CacheModified = true;
                Dictionary<string, string> Section;
                if (!m_Sections.TryGetValue(SectionName, out Section))
                {
                    Section = new Dictionary<string, string>();
                    m_Sections.Add(SectionName, Section);
                }
                if (Section.ContainsKey(Key)) Section.Remove(Key);
                Section.Add(Key, Convert.ToString(Value));
            }

            Flush();
            return true;
        }

        public string EnCodeByteArray(byte[] Value)
        {
            if (Value == null) return null;
            StringBuilder sb = new StringBuilder();
            foreach (byte b in Value)
            {
                string hex = Convert.ToString(b, 16);
                int l = hex.Length;
                if (l > 2)
                {
                    sb.Append(hex.Substring(l - 2, 2));
                }
                else
                {
                    if (l < 2) sb.Append("0");
                    sb.Append(hex);
                }
            }
            return sb.ToString();
        }

        private byte[] DeCodeByteArray(string Value)
        {
            if (Value == null) return null;
            int l = Value.Length;
            if (l < 2) return new byte[] { };
            l /= 2;
            byte[] Result = new byte[l];
            for (int i = 0; i < l; i++) Result[i] = Convert.ToByte(Value.Substring(i * 2, 2), 16);
            return Result;
        }

    }
}
