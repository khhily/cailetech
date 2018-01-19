using System;

namespace CL.View.Entity.Other
{
    /// <summary>
    /// 站点配置实体类
    /// </summary>
    [Serializable]
    public class SiteConfig
    {
        public SiteConfig() { }

        private string _webname = "";
        private string _weburl = "";
        private string _webcompany = "";
        private string _webaddress = "";
        private string _webtel = "";
        private string _webfax = "";
        private string _webmail = "";
        private string _webcrod = "";

        private string _webpath = "";
        private string _webmanagepath = "";
        private int _logstatus = 0;
        private int _webstatus = 1;
        private string _webclosereason = "";
        private int _staticstatus = 0;
        private string _staticextension = "";

        private string _smsapiurl = "";
        private string _smsusername = "";
        private string _smspassword = "";

        private string _emailsmtp = "";
        private int _emailssl = 0;
        private int _emailport = 25;
        private string _emailfrom = "";
        private string _emailusername = "";
        private string _emailpassword = "";
        private string _emailnickname = "";

        private string _filepath = "";
        private int _filesave = 1;
        private string _fileextension = "";
        private string _videoextension = "";
        private int _attachsize = 0;
        private int _videosize = 0;
        private int _imgsize = 0;
        private int _imgmaxheight = 0;
        private int _imgmaxwidth = 0;
        private int _thumbnailheight = 0;
        private int _thumbnailwidth = 0;
        private int _watermarktype = 0;
        private int _watermarkposition = 9;
        private int _watermarkimgquality = 80;
        private string _watermarkpic = "";
        private int _watermarktransparency = 10;
        private string _watermarktext = "";
        private string _watermarkfont = "";
        private int _watermarkfontsize = 12;

        private string _neteaseappkey = "6d358ec7e08910d12c63394a7a6d5bff";
        private string _neteaseappsecret = "aacf370ef68c";
        private string _getchatroom = "";
        private string _createchatroom = "";
        private string _updatechatroom = "";
        private string _setmemberrole = "";
        private string _getuinfos = "";
        private string _updateuinfo = "";
        private string _createuser = "";

        private string _baiduappkey = "E5GwTF5GWCfUC5GwH1w9qXAs";
        private string _baidusecretkey = "i4rLpTvTYmuwEud3kQ5r6qcFApV5GCIc";
        private string _baiduurl = "http://api.tuisong.baidu.com/rest/3.0/";

        private string _hyagenterid = "10000910";
        private string _hyagenterpwd = "a361b66561a0b99ff6c553badf1016d2";
        private string _hyusername = "10000910";
        private string _hyaddress = "http://b2blib.198fc.cn:8080/b2blib/lotteryxml.php";

        private long _maxwinmoneynotice = 0;
        private long _interfacemoneywarning = 0;
        private int _regwarning = 0;
        private string _noticetel = "";

        #region 主站基本信息==================================
        /// <summary>
        /// 网站名称
        /// </summary>
        public string webname
        {
            get { return _webname; }
            set { _webname = value; }
        }
        /// <summary>
        /// 网站域名
        /// </summary>
        public string weburl
        {
            get { return _weburl; }
            set { _weburl = value; }
        }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string webcompany
        {
            get { return _webcompany; }
            set { _webcompany = value; }
        }
        /// <summary>
        /// 通讯地址
        /// </summary>
        public string webaddress
        {
            get { return _webaddress; }
            set { _webaddress = value; }
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string webtel
        {
            get { return _webtel; }
            set { _webtel = value; }
        }
        /// <summary>
        /// 传真号码
        /// </summary>
        public string webfax
        {
            get { return _webfax; }
            set { _webfax = value; }
        }
        /// <summary>
        /// 管理员邮箱
        /// </summary>
        public string webmail
        {
            get { return _webmail; }
            set { _webmail = value; }
        }
        /// <summary>
        /// 网站备案号
        /// </summary>
        public string webcrod
        {
            get { return _webcrod; }
            set { _webcrod = value; }
        }
        #endregion

        #region 功能权限设置==================================
        /// <summary>
        /// 网站安装目录
        /// </summary>
        public string webpath
        {
            get { return _webpath; }
            set { _webpath = value; }
        }
        /// <summary>
        /// 网站管理目录
        /// </summary>
        public string webmanagepath
        {
            get { return _webmanagepath; }
            set { _webmanagepath = value; }
        }
        /// <summary>
        /// 后台管理日志
        /// </summary>
        public int logstatus
        {
            get { return _logstatus; }
            set { _logstatus = value; }
        }
        /// <summary>
        /// 是否关闭网站
        /// </summary>
        public int webstatus
        {
            get { return _webstatus; }
            set { _webstatus = value; }
        }
        /// <summary>
        /// 关闭原因描述
        /// </summary>
        public string webclosereason
        {
            get { return _webclosereason; }
            set { _webclosereason = value; }
        }
        /// <summary>
        /// 检查网站重写状态 0.关闭 1.伪静态 2.静态
        /// </summary>
        public int staticstatus
        {
            get { return _staticstatus; }
            set { _staticstatus = value; }
        }
        /// <summary>
        /// 生成静态扩展名
        /// </summary>
        public string staticextension
        {
            get { return _staticextension; }
            set { _staticextension = value; }
        }
        #endregion

        #region 短信平台设置==================================
        /// <summary>
        /// 短信API地址
        /// </summary>
        public string smsapiurl
        {
            get { return _smsapiurl; }
            set { _smsapiurl = value; }
        }
        /// <summary>
        /// 短信平台登录账户名
        /// </summary>
        public string smsusername
        {
            get { return _smsusername; }
            set { _smsusername = value; }
        }
        /// <summary>
        /// 短信平台登录密码
        /// </summary>
        public string smspassword
        {
            get { return _smspassword; }
            set { _smspassword = value; }
        }
        #endregion

        #region 邮件发送设置==================================
        /// <summary>
        /// STMP服务器
        /// </summary>
        public string emailsmtp
        {
            get { return _emailsmtp; }
            set { _emailsmtp = value; }
        }
        /// <summary>
        /// 是否启用SSL加密连接
        /// </summary>
        public int emailssl
        {
            get { return _emailssl; }
            set { _emailssl = value; }
        }
        /// <summary>
        /// SMTP端口
        /// </summary>
        public int emailport
        {
            get { return _emailport; }
            set { _emailport = value; }
        }
        /// <summary>
        /// 发件人地址
        /// </summary>
        public string emailfrom
        {
            get { return _emailfrom; }
            set { _emailfrom = value; }
        }
        /// <summary>
        /// 邮箱账号
        /// </summary>
        public string emailusername
        {
            get { return _emailusername; }
            set { _emailusername = value; }
        }
        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string emailpassword
        {
            get { return _emailpassword; }
            set { _emailpassword = value; }
        }
        /// <summary>
        /// 发件人昵称
        /// </summary>
        public string emailnickname
        {
            get { return _emailnickname; }
            set { _emailnickname = value; }
        }
        #endregion

        #region 文件上传设置==================================
        /// <summary>
        /// 附件上传目录
        /// </summary>
        public string filepath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }
        /// <summary>
        /// 附件保存方式
        /// </summary>
        public int filesave
        {
            get { return _filesave; }
            set { _filesave = value; }
        }
        /// <summary>
        /// 附件上传类型
        /// </summary>
        public string fileextension
        {
            get { return _fileextension; }
            set { _fileextension = value; }
        }
        /// <summary>
        /// 视频上传类型
        /// </summary>
        public string videoextension
        {
            get { return _videoextension; }
            set { _videoextension = value; }
        }
        /// <summary>
        /// 文件上传大小
        /// </summary>
        public int attachsize
        {
            get { return _attachsize; }
            set { _attachsize = value; }
        }
        /// <summary>
        /// 视频上传大小
        /// </summary>
        public int videosize
        {
            get { return _videosize; }
            set { _videosize = value; }
        }
        /// <summary>
        /// 图片上传大小
        /// </summary>
        public int imgsize
        {
            get { return _imgsize; }
            set { _imgsize = value; }
        }
        /// <summary>
        /// 图片最大高度(像素)
        /// </summary>
        public int imgmaxheight
        {
            get { return _imgmaxheight; }
            set { _imgmaxheight = value; }
        }
        /// <summary>
        /// 图片最大宽度(像素)
        /// </summary>
        public int imgmaxwidth
        {
            get { return _imgmaxwidth; }
            set { _imgmaxwidth = value; }
        }
        /// <summary>
        /// 生成缩略图高度(像素)
        /// </summary>
        public int thumbnailheight
        {
            get { return _thumbnailheight; }
            set { _thumbnailheight = value; }
        }
        /// <summary>
        /// 生成缩略图宽度(像素)
        /// </summary>
        public int thumbnailwidth
        {
            get { return _thumbnailwidth; }
            set { _thumbnailwidth = value; }
        }
        /// <summary>
        /// 图片水印类型
        /// </summary>
        public int watermarktype
        {
            get { return _watermarktype; }
            set { _watermarktype = value; }
        }
        /// <summary>
        /// 图片水印位置
        /// </summary>
        public int watermarkposition
        {
            get { return _watermarkposition; }
            set { _watermarkposition = value; }
        }
        /// <summary>
        /// 图片生成质量
        /// </summary>
        public int watermarkimgquality
        {
            get { return _watermarkimgquality; }
            set { _watermarkimgquality = value; }
        }
        /// <summary>
        /// 图片水印文件
        /// </summary>
        public string watermarkpic
        {
            get { return _watermarkpic; }
            set { _watermarkpic = value; }
        }
        /// <summary>
        /// 水印透明度
        /// </summary>
        public int watermarktransparency
        {
            get { return _watermarktransparency; }
            set { _watermarktransparency = value; }
        }
        /// <summary>
        /// 水印文字
        /// </summary>
        public string watermarktext
        {
            get { return _watermarktext; }
            set { _watermarktext = value; }
        }
        /// <summary>
        /// 文字字体
        /// </summary>
        public string watermarkfont
        {
            get { return _watermarkfont; }
            set { _watermarkfont = value; }
        }
        /// <summary>
        /// 文字大小(像素)
        /// </summary>
        public int watermarkfontsize
        {
            get { return _watermarkfontsize; }
            set { _watermarkfontsize = value; }
        }
        #endregion

        #region 网易云信接口
        /// <summary>
        /// AppKey
        /// </summary>
        public string neteaseappkey
        {
            get { return _neteaseappkey; }
            set { _neteaseappkey = value; }
        }
        /// <summary>
        /// App秘密
        /// </summary>
        public string neteaseappsecret
        {
            get { return _neteaseappsecret; }
            set { _neteaseappsecret = value; }
        }
        /// <summary>
        /// 创建聊天室接口地址
        /// </summary>
        public string getchatroom
        {
            get { return _getchatroom; }
            set { _getchatroom = value; }
        }
        /// <summary>
        /// 获取聊天室信息地址
        /// </summary>
        public string createchatroom
        {
            get { return _createchatroom; }
            set { _createchatroom = value; }
        }
        /// <summary>
        /// 更新聊天室
        /// </summary>
        public string updatechatroom
        {
            get { return _updatechatroom; }
            set { _updatechatroom = value; }
        }
        /// <summary>
        /// 设置聊天室内用户角色
        /// </summary>
        public string setmemberrole
        {
            get { return _setmemberrole; }
            set { _setmemberrole = value; }
        }
        /// <summary>
        /// 获取用户名片
        /// </summary>
        public string getuinfos
        {
            get { return _getuinfos; }
            set { _getuinfos = value; }
        }
        /// <summary>
        /// 更新用户名片
        /// </summary>
        public string updateuinfo
        {
            get { return _updateuinfo; }
            set { _updateuinfo = value; }
        }
        /// <summary>
        /// 创建云信ID
        /// </summary>
        public string createuser
        {
            get { return _createuser; }
            set { _createuser = value; }
        }
        #endregion

        #region 百度推送
        /// <summary>
        /// 百度推送AppKey
        /// </summary>
        public string baiduappkey
        {
            get { return _baiduappkey; }
            set { _baiduappkey = value; }
        }
        /// <summary>
        /// 百度推送SecretKey
        /// </summary>
        public string baidusecretkey
        {
            get { return _baidusecretkey; }
            set { _baidusecretkey = value; }
        }
        /// <summary>
        /// 百度推送API地址
        /// </summary>
        public string baiduurl
        {
            get { return _baiduurl; }
            set { _baiduurl = value; }
        }
        #endregion

        #region 华阳接口
        /// <summary>
        /// 华阳接口代理ID
        /// </summary>
        public string HYAgenterId
        {
            get { return _hyagenterid; }
            set { _hyagenterid = value; }
        }
        /// <summary>
        /// 华阳接口代理账号密码
        /// </summary>
        public string HYAgenterPwd
        {
            get { return _hyagenterpwd; }
            set { _hyagenterpwd = value; }
        }
        /// <summary>
        /// 华阳接口代理账号
        /// </summary>
        public string HYUserName
        {
            get { return _hyusername; }
            set { _hyusername = value; }
        }
        /// <summary>
        /// 华阳接口地址
        /// </summary>
        public string HYAddress
        {
            get { return _hyaddress; }
            set { _hyaddress = value; }
        }
        #endregion

        #region 其他配置
        /// <summary>
        /// 最大中奖金额提醒
        /// </summary>
        public long MaxWinMoneyNotice
        {
            get { return _maxwinmoneynotice; }
            set { _maxwinmoneynotice = value; }
        }
        /// <summary>
        /// 接口余额预警提醒
        /// </summary>
        public long InterfaceMoneyWarning
        {
            get { return _interfacemoneywarning; }
            set { _interfacemoneywarning = value; }
        }
        /// <summary>
        /// 预注册账号预警提醒
        /// </summary>
        public int RegWarning
        {
            get { return _regwarning; }
            set { _regwarning = value; }
        }
        /// <summary>
        /// 提醒号码
        /// </summary>
        public string NoticeTel
        {
            get { return _noticetel; }
            set { _noticetel = value; }
        }
        #endregion

    }
}
