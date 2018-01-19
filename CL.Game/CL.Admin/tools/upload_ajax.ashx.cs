using CL.Admin.UI;
using CL.SystemInfo.BLL;
using CL.Tools;
using CL.Tools.Common;
using CL.View.Entity.Other;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace Admin.tools
{
    /// <summary>
    /// upload_ajax 的摘要说明
    /// </summary>
    public class upload_ajax : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            //检查管理员是否登录
            if (!new AdminPage().IsAdminLogin())
            {
                context.Response.Write("{\"status\": 0, \"msg\": \"尚未登录或已超时，请登录后操作！\"}");
                return;
            }
            //取得处事类型
            string action = QPRequest.GetQueryString("action");

            switch (action)
            {
                case "EditorFile": //编辑器文件
                    EditorFile(context);
                    break;
                case "ManagerFile": //管理文件
                    ManagerFile(context);
                    break;
                case "UpLoadifyFile": //管理文件
                    UpLoadifyFile(context);
                    break;
                case "UpLoadifyExcelTOLst": //管理文件
                    UpLoadifyExcelTOLst(context);
                    break;
                case "activity": //管理文件
                    Activity(context);
                    break;
                default: //普通上传
                    UpLoadFile(context);
                    break;
            }

        }

        #region 活动图片上传
        protected void Activity(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "text/plain";
                //获取上传的文件
                HttpPostedFile httpPostedFile = context.Request.Files["uploadify"];

                //如果接收到文件则httpPostedFile不为null，则对上传的文件进行处理，否则向客户端返回0
                if (httpPostedFile != null)
                {
                    //获取文件名
                    string strfileName = httpPostedFile.FileName;

                    //获取扩展名
                    string strExt = Path.GetExtension(strfileName);

                    //允许上传的文件类型
                    string[] strExts = { ".jpg", ".png", ".gif" };

                    //如果上传的文件类型，在被允许的类型中，则保存，否则向客户端输出“不允许上传”的信息提示。
                    if (strExts.Contains(strExt))
                    {
                        string strSaveName = string.Empty;
                        string strSavePath = ConvertImageByWH(context, httpPostedFile.InputStream, strExt, out strSaveName);

                        string strJson = strSavePath;
                        //将文件的保存的相对路径输出到客户端
                        context.Response.Write(strJson);
                    }
                    else
                    {
                        context.Response.Write("");
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }
        }
        /// <summary>
        /// 按照给定的宽高对图片进行压缩
        /// </summary>
        /// <param name="byteImg">图片字节流</param>
        /// <param name="intImgCompressWidth">压缩后的图片宽度</param>
        /// <param name="intImgCompressHeight">压缩后的图片高度</param>
        /// <param name="strExt">扩展名</param>
        /// <param name="strSaveName">保存后的名字</param>
        /// <returns>压缩后的图片相对路径</returns>
        private string ConvertImageByWH(HttpContext context, Stream stream, string strExt, out string strSaveName)
        {
            //从输入流中获取上传的image对象
            using (Image img = Image.FromStream(stream))
            {
                //根据压缩比例求出图片的宽度
                int intWidth = 1020;
                int intHeight = 225;
                //画布
                using (Bitmap bitmap = new Bitmap(img, new Size(intWidth, intHeight)))
                {
                    //在画布上创建画笔对象
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        //将图片使用压缩后的宽高,从0，0位置画在画布上
                        graphics.DrawImage(img, 0, 0, intWidth, intHeight);
                        //创建保存路径
                        string strSavePath = string.Format("{0}{1}", ConfigHelper.GetConfigString("ICON").TrimEnd('\\'), "\\activit\\");
                        string strSaveUrl = string.Format("{0}{1}", ConfigHelper.GetConfigString("ICONHOSTURL").TrimEnd('/'), "/activit/");

                        //如果保存目录不存在，则创建
                        if (!Directory.Exists(strSavePath))
                        {
                            Directory.CreateDirectory(strSavePath);
                        }
                        //定义新的文件名
                        string strfileNameNoExt = Guid.NewGuid().ToString();
                        strSaveName = strfileNameNoExt + strExt;
                        //添加时如果图片已经存在则删除
                        if (File.Exists(strSavePath + strSaveName))
                        {
                            File.Delete(strSavePath + strSaveName);
                        }
                        //保存文件
                        bitmap.Save(strSavePath + strSaveName);
                        return strSaveUrl + strSaveName;
                    }
                }
            }
        }
        #endregion

        #region Uploadify上传文件处理===================================
        private void UpLoadifyFile(HttpContext context)
        {
            string fileSign = QPRequest.GetQueryString("fileSign");
            SiteConfig siteConfig = new SiteConfigBLL().loadConfig();
            string _delfile = QPRequest.GetString("DelFilePath");
            HttpPostedFile _upfile = context.Request.Files["context"];
            bool _iswater = false; //默认不打水印
            bool _isthumbnail = false; //默认不生成缩略图

            if (QPRequest.GetQueryString("IsWater") == "1")
                _iswater = true;
            if (QPRequest.GetQueryString("IsThumbnail") == "1")
                _isthumbnail = true;
            if (_upfile == null)
            {
                context.Response.Write("{\"status\": 0, \"msg\": \"请选择要上传文件！\"}");
                return;
            }
            UpLoad upFiles = new UpLoad();
            string msg = upFiles.fileSaveAsUpLoadify(_upfile, fileSign, _isthumbnail, _iswater);
            //删除已存在的旧文件，旧文件不为空且应是上传文件，防止跨目录删除
            if (!string.IsNullOrEmpty(_delfile) && _delfile.IndexOf("../") == -1
                && _delfile.ToLower().StartsWith(siteConfig.webpath.ToLower() + siteConfig.filepath.ToLower()))
            {
                Utils.DeleteUpFile(_delfile);
            }
            //返回成功信息
            context.Response.Write(msg);
            context.Response.End();
        }
        #endregion


        #region Uploadify上传文件处理===================================
        private void UpLoadifyExcelTOLst(HttpContext context)
        {
            SiteConfig siteConfig = new SiteConfigBLL().loadConfig();
            string _delfile = QPRequest.GetString("DelFilePath");
            HttpPostedFile _upfile = context.Request.Files["context"];
            bool _iswater = false; //默认不打水印
            bool _isthumbnail = false; //默认不生成缩略图

            if (QPRequest.GetQueryString("IsWater") == "1")
                _iswater = true;
            if (QPRequest.GetQueryString("IsThumbnail") == "1")
                _isthumbnail = true;
            if (_upfile == null)
            {
                context.Response.Write("{\"status\": 0, \"msg\": \"请选择要上传文件！\"}");
                return;
            }
            UpLoad upFiles = new UpLoad();
            string msg = upFiles.ExcelTOLst(_upfile, _isthumbnail, _iswater);
            //删除已存在的旧文件，旧文件不为空且应是上传文件，防止跨目录删除
            if (!string.IsNullOrEmpty(_delfile) && _delfile.IndexOf("../") == -1
                && _delfile.ToLower().StartsWith(siteConfig.webpath.ToLower() + siteConfig.filepath.ToLower()))
            {
                Utils.DeleteUpFile(_delfile);
            }
            //返回成功信息
            context.Response.Write(msg);
            context.Response.End();
        }
        #endregion



        #region 上传文件处理===================================
        private void UpLoadFile(HttpContext context)
        {
            SiteConfig siteConfig = new SiteConfigBLL().loadConfig();
            string _delfile = QPRequest.GetString("DelFilePath");
            HttpPostedFile _upfile = context.Request.Files["context"];
            bool _iswater = false; //默认不打水印
            bool _isthumbnail = false; //默认不生成缩略图

            if (QPRequest.GetQueryString("IsWater") == "1")
                _iswater = true;
            if (QPRequest.GetQueryString("IsThumbnail") == "1")
                _isthumbnail = true;
            if (_upfile == null)
            {
                context.Response.Write("{\"status\": 0, \"msg\": \"请选择要上传文件！\"}");
                return;
            }
            UpLoad upFiles = new UpLoad();
            string msg = upFiles.fileSaveAs(_upfile, _isthumbnail, _iswater);
            //删除已存在的旧文件，旧文件不为空且应是上传文件，防止跨目录删除
            if (!string.IsNullOrEmpty(_delfile) && _delfile.IndexOf("../") == -1
                && _delfile.ToLower().StartsWith(siteConfig.webpath.ToLower() + siteConfig.filepath.ToLower()))
            {
                Utils.DeleteUpFile(_delfile);
            }
            //返回成功信息
            context.Response.Write(msg);
            context.Response.End();
        }
        #endregion

        #region 编辑器上传处理===================================
        private void EditorFile(HttpContext context)
        {
            bool _iswater = false; //默认不打水印
            if (context.Request.QueryString["IsWater"] == "1")
                _iswater = true;
            HttpPostedFile imgFile = context.Request.Files["imgFile"];
            if (imgFile == null)
            {
                showError(context, "请选择要上传文件！");
                return;
            }
            UpLoad upFiles = new UpLoad();
            string remsg = upFiles.fileSaveAs(imgFile, false, _iswater);
            Dictionary<string, object> dic = JsonHelper.DataRowFromJSON(remsg);
            string status = dic["status"].ToString();
            string msg = dic["msg"].ToString();
            if (status == "0")
            {
                showError(context, msg);
                return;
            }
            string filePath = dic["path"].ToString(); //取得上传后的路径
            Hashtable hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = filePath;
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(JsonHelper.ObjectToJSON(hash));
            context.Response.End();
        }
        //显示错误
        private void showError(HttpContext context, string message)
        {
            Hashtable hash = new Hashtable();
            hash["error"] = 1;
            hash["message"] = message;
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(JsonHelper.ObjectToJSON(hash));
            context.Response.End();
        }
        #endregion

        #region 浏览文件处理=====================================
        private void ManagerFile(HttpContext context)
        {
            SiteConfig siteConfig = new SiteConfigBLL().loadConfig();

            //根目录路径，相对路径
            String rootPath = siteConfig.webpath + siteConfig.filepath + "/"; //站点目录+上传目录
            //根目录URL，可以指定绝对路径，比如 http://www.yoursite.com/attached/
            String rootUrl = siteConfig.webpath + siteConfig.filepath + "/";
            //图片扩展名
            String fileTypes = "gif,jpg,jpeg,png,bmp";

            String currentPath = "";
            String currentUrl = "";
            String currentDirPath = "";
            String moveupDirPath = "";

            String dirPath = Utils.GetMapPath(rootPath);

            //根据path参数，设置各路径和URL
            String path = context.Request.QueryString["path"];
            path = String.IsNullOrEmpty(path) ? "" : path;
            if (path == "")
            {
                currentPath = dirPath;
                currentUrl = rootUrl;
                currentDirPath = "";
                moveupDirPath = "";
            }
            else
            {
                currentPath = dirPath + path;
                currentUrl = rootUrl + path;
                currentDirPath = path;
                moveupDirPath = Regex.Replace(currentDirPath, @"(.*?)[^\/]+\/$", "$1");
            }

            //排序形式，name or size or type
            String order = context.Request.QueryString["order"];
            order = String.IsNullOrEmpty(order) ? "" : order.ToLower();

            //不允许使用..移动到上一级目录
            if (Regex.IsMatch(path, @"\.\."))
            {
                context.Response.Write("Access is not allowed.");
                context.Response.End();
            }
            //最后一个字符不是/
            if (path != "" && !path.EndsWith("/"))
            {
                context.Response.Write("Parameter is not valid.");
                context.Response.End();
            }
            //目录不存在或不是目录
            if (!Directory.Exists(currentPath))
            {
                context.Response.Write("Directory does not exist.");
                context.Response.End();
            }

            //遍历目录取得文件信息
            string[] dirList = Directory.GetDirectories(currentPath);
            string[] fileList = Directory.GetFiles(currentPath);

            switch (order)
            {
                case "size":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new SizeSorter());
                    break;
                case "type":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new TypeSorter());
                    break;
                case "name":
                default:
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new NameSorter());
                    break;
            }

            Hashtable result = new Hashtable();
            result["moveup_dir_path"] = moveupDirPath;
            result["current_dir_path"] = currentDirPath;
            result["current_url"] = currentUrl;
            result["total_count"] = dirList.Length + fileList.Length;
            List<Hashtable> dirFileList = new List<Hashtable>();
            result["file_list"] = dirFileList;
            for (int i = 0; i < dirList.Length; i++)
            {
                DirectoryInfo dir = new DirectoryInfo(dirList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = true;
                hash["has_file"] = (dir.GetFileSystemInfos().Length > 0);
                hash["filesize"] = 0;
                hash["is_photo"] = false;
                hash["filetype"] = "";
                hash["filename"] = dir.Name;
                hash["datetime"] = dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            for (int i = 0; i < fileList.Length; i++)
            {
                FileInfo file = new FileInfo(fileList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = false;
                hash["has_file"] = false;
                hash["filesize"] = file.Length;
                hash["is_photo"] = (Array.IndexOf(fileTypes.Split(','), file.Extension.Substring(1).ToLower()) >= 0);
                hash["filetype"] = file.Extension.Substring(1);
                hash["filename"] = file.Name;
                hash["datetime"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            context.Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
            context.Response.Write(JsonHelper.ObjectToJSON(result));
            context.Response.End();
        }

        #region Helper
        public class NameSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.FullName.CompareTo(yInfo.FullName);
            }
        }

        public class SizeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.Length.CompareTo(yInfo.Length);
            }
        }

        public class TypeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.Extension.CompareTo(yInfo.Extension);
            }
        }
        #endregion
        #endregion


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}