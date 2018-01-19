using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace CL.Tools.Common
{
    public class ImageClass
    {
        public Image ResourceImage;
        private int ImageWidth;
        private int ImageHeight;

        public string ErrMessage;

        /// <summary>
        /// 类的构造函数
        /// </summary>
        /// <param name="ImageFileName">图片文件的全路径名称</param>
        public ImageClass(string ImageFileName)
        {
            if (!string.IsNullOrEmpty(ImageFileName))
                ResourceImage = Image.FromFile(ImageFileName);
            ErrMessage = "";
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        /// <summary>
        /// 生成缩略图重载方法1，返回缩略图的Image对象
        /// </summary>
        /// <param name="Width">缩略图的宽度</param>
        /// <param name="Height">缩略图的高度</param>
        /// <returns>缩略图的Image对象</returns>
        public Image GetReducedImage(int Width, int Height)
        {
            try
            {
                Image ReducedImage;

                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                ReducedImage = ResourceImage.GetThumbnailImage(Width, Height, callb, IntPtr.Zero);

                return ReducedImage;
            }
            catch (Exception e)
            {
                ErrMessage = e.Message;
                return null;
            }
        }

        /// <summary>
        /// 生成缩略图重载方法2，将缩略图文件保存到指定的路径
        /// </summary>
        /// <param name="Width">缩略图的宽度</param>
        /// <param name="Height">缩略图的高度</param>
        /// <param name="targetFilePath">缩略图保存的全文件名，(带路径)，参数格式：D:\Images\filename.jpg</param>
        /// <returns>成功返回true，否则返回false</returns>
        public bool GetReducedImage(int Width, int Height, string targetFilePath)
        {
            try
            {
                Image ReducedImage;

                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                ReducedImage = ResourceImage.GetThumbnailImage(Width, Height, callb, IntPtr.Zero);
                ReducedImage.Save(@targetFilePath, ImageFormat.Jpeg);

                ReducedImage.Dispose();

                return true;
            }
            catch (Exception e)
            {
                ErrMessage = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 生成缩略图重载方法3，返回缩略图的Image对象
        /// </summary>
        /// <param name="Percent">缩略图的宽度百分比 如：需要百分之80，就填0.8</param>  
        /// <returns>缩略图的Image对象</returns>
        public Image GetReducedImage(double Percent)
        {
            try
            {
                Image ReducedImage;

                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                ImageWidth = Convert.ToInt32(ResourceImage.Width * Percent);
                ImageHeight = Convert.ToInt32(ResourceImage.Width * Percent);

                ReducedImage = ResourceImage.GetThumbnailImage(ImageWidth, ImageHeight, callb, IntPtr.Zero);

                return ReducedImage;
            }
            catch (Exception e)
            {
                ErrMessage = e.Message;
                return null;
            }
        }

        /// <summary>
        /// 生成缩略图重载方法4，返回缩略图的Image对象
        /// </summary>
        /// <param name="Percent">缩略图的宽度百分比 如：需要百分之80，就填0.8</param>  
        /// <param name="targetFilePath">缩略图保存的全文件名，(带路径)，参数格式：D:\Images\filename.jpg</param>
        /// <returns>成功返回true,否则返回false</returns>
        public bool GetReducedImage(double Percent, string targetFilePath)
        {
            try
            {
                Image ReducedImage;

                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                ImageWidth = Convert.ToInt32(ResourceImage.Width * Percent);
                ImageHeight = Convert.ToInt32(ResourceImage.Width * Percent);

                ReducedImage = ResourceImage.GetThumbnailImage(ImageWidth, ImageHeight, callb, IntPtr.Zero);

                ReducedImage.Save(@targetFilePath, ImageFormat.Jpeg);

                ReducedImage.Dispose();

                return true;
            }
            catch (Exception e)
            {
                ErrMessage = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 保存缩略图
        /// (URL重写后的图片地址)
        /// </summary>
        /// <param name="SavePath">保存地址</param>
        /// <param name="RewritePath">URL重写后的图片地址</param>
        /// <param name="Percent">图片缩放比例</param>
        /// <returns></returns>
        public bool ThumbnailImage(string SavePath, string RewritePath, double Percent)
        {
            try
            {
                //初始化URL
                Uri uri = new Uri(RewritePath);
                //接收URL资源数据
                WebClient client = new WebClient();
                byte[] data = client.DownloadData(uri.AbsoluteUri);
                //初始化数据流
                MemoryStream ms = new MemoryStream(data);
                //数据流创建Image对象
                Image ResourceImage_Rewrite = Image.FromStream(ms);
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                #region 计算
                if (ResourceImage_Rewrite.Width <= 100 || ResourceImage_Rewrite.Height <= 100)
                    Percent = 0.6;
                else if ((ResourceImage_Rewrite.Width > 100 && ResourceImage_Rewrite.Width <= 200) || (ResourceImage_Rewrite.Height > 100 && ResourceImage_Rewrite.Height <= 200))
                    Percent = 0.4;
                else if ((ResourceImage_Rewrite.Width > 200 && ResourceImage_Rewrite.Width <= 400) || (ResourceImage_Rewrite.Height > 200 && ResourceImage_Rewrite.Height <= 400))
                    Percent = 0.25;
                else if (ResourceImage_Rewrite.Width > 400 || ResourceImage_Rewrite.Height > 400)
                    Percent = 0.20;

                if (ResourceImage_Rewrite.Width <= 50 && ResourceImage_Rewrite.Height <= 50)
                    Percent = 1;

                #endregion
                //生成缩放图
                int ImageWidth = Convert.ToInt32(ResourceImage_Rewrite.Width * Percent);
                int ImageHeight = Convert.ToInt32(ResourceImage_Rewrite.Width * Percent);
                ReducedImage = ResourceImage_Rewrite.GetThumbnailImage(ImageWidth, ImageHeight, callb, IntPtr.Zero);
                //保存到磁盘
                ReducedImage.Save(SavePath, ImageFormat.Jpeg);
                ReducedImage.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ErrMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 上载图片指定缓冲区
        /// (URL重写后的图片地址)
        /// </summary>
        /// <param name="SaveHttp">缓冲区地址</param>
        /// <param name="RewritePath">图片地址(URL重写后的图片地址)</param>
        /// <param name="Percent">图片缩放比例</param>
        /// <returns></returns>
        public bool HttpThumbnailImage(string SaveHttp, string RewritePath, double Percent)
        {
            try
            {
                //初始化URL
                Uri uri = new Uri(RewritePath);
                //接收URL资源数据
                WebClient client = new WebClient();
                byte[] data = client.DownloadData(uri.AbsoluteUri);
                //初始化数据流
                MemoryStream ms = new MemoryStream(data);
                //数据流创建Image对象
                Image ResourceImage_Rewrite = Image.FromStream(ms);
                Image ReducedImage;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                #region 计算
                if (ResourceImage_Rewrite.Width <= 100 || ResourceImage_Rewrite.Height <= 100)
                    Percent = 0.6;
                else if ((ResourceImage_Rewrite.Width > 100 && ResourceImage_Rewrite.Width <= 200) || (ResourceImage_Rewrite.Height > 100 && ResourceImage_Rewrite.Height <= 200))
                    Percent = 0.4;
                else if ((ResourceImage_Rewrite.Width > 200 && ResourceImage_Rewrite.Width <= 400) || (ResourceImage_Rewrite.Height > 200 && ResourceImage_Rewrite.Height <= 400))
                    Percent = 0.25;
                else if (ResourceImage_Rewrite.Width > 400 || ResourceImage_Rewrite.Height > 400)
                    Percent = 0.20;
                #endregion
                //生成缩放图
                int ImageWidth = Convert.ToInt32(ResourceImage_Rewrite.Width * Percent);
                int ImageHeight = Convert.ToInt32(ResourceImage_Rewrite.Width * Percent);
                ReducedImage = ResourceImage_Rewrite.GetThumbnailImage(ImageWidth, ImageHeight, callb, IntPtr.Zero);

                //http上传指定数据
                uri = new Uri(SaveHttp);
                //图片转换流
                MemoryStream stream = new MemoryStream();
                ReducedImage.Save(stream, ImageFormat.Jpeg);
                byte[] mydata = new byte[stream.Length];
                mydata = stream.ToArray();
                //上载指定缓冲区
                client.UploadData(uri, mydata);

                stream.Close();
                return true;
            }
            catch (Exception ex)
            {
                ErrMessage = ex.Message;
                return false;
            }
        }
    }
}
