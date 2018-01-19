using CL.Enum.Common.Push.JiGuang;
using cn.jpush.api;
using cn.jpush.api.push;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;
using System;

namespace CL.Tools.JiGuangPush
{
    public class PushHelper
    {
        protected string appKey = System.Configuration.ConfigurationManager.AppSettings["JIGUANGAPPKEY"] ?? "";
        protected string masterSecret = System.Configuration.ConfigurationManager.AppSettings["JIGUANGMASTERSECRET"] ?? "";

        protected JPushClient client = null;
        /// <summary>
        /// 
        /// </summary>
        public PushHelper()
        {
            client = new JPushClient(appKey, masterSecret);
        }

        /// <summary>
        /// 广播全网公告
        /// </summary>
        /// <param name="Alert">广播行为</param>
        /// <param name="Msg">自定义消息</param>
        /// <param name="result">回调数据</param>
        /// <returns></returns>
        public bool BroadcastNotice(string Title, string Msg, ref MessageResult result)
        {
            try
            {
                bool ResetOptionsApnsProduction = true;
                bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["RESETOPTIONSAPNSPRODUCTION"], out ResetOptionsApnsProduction);

                if (client == null)
                    client = new JPushClient(appKey, masterSecret);
                PushPayload pushPayload = new PushPayload();
                pushPayload.platform = Platform.all();
                pushPayload.audience = Audience.all();
                var notification = new Notification();
                #region Android
                var androidNotification = new AndroidNotification();
                androidNotification.setAlert(Msg);
                androidNotification.setTitle(Title);
                notification.AndroidNotification = androidNotification;
                #endregion
                #region IO
                var iosNotification = new IosNotification();
                iosNotification.setAlert(Msg);
                iosNotification.setBadge(1);
                iosNotification.setSound("default");
                
                notification.IosNotification = iosNotification;
                
                #endregion
                pushPayload.notification = notification;
                pushPayload.message = Message.content(Msg);
                pushPayload.ResetOptionsApnsProduction(ResetOptionsApnsProduction);
                result = client.SendPush(pushPayload);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("广播全网公告错误[BroadcastNotice]：{0}", ex.Message));
            }
        }
        /// <summary>
        /// 广播IOS公告
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Msg"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool BroadcastNotice_Ios(string Title, string Msg, ref MessageResult result)
        {
            bool ResetOptionsApnsProduction = true;
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["RESETOPTIONSAPNSPRODUCTION"], out ResetOptionsApnsProduction);

            if (client == null)
                client = new JPushClient(appKey, masterSecret);
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.ios();
            pushPayload.audience = Audience.all();
            var notification = new Notification();
            #region IO
            var iosNotification = new IosNotification();
            iosNotification.setAlert(Msg);
            iosNotification.setBadge(1);
            iosNotification.setSound("default");
            notification.IosNotification = iosNotification;
            #endregion
            pushPayload.notification = notification;
            pushPayload.message = Message.content(Msg);
            pushPayload.ResetOptionsApnsProduction(ResetOptionsApnsProduction);
            result = client.SendPush(pushPayload);
            return true;
        }
        /// <summary>
        /// 广播ANDROID公告
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Msg"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool BroadcastNotice_Android(string Title, string Msg, ref MessageResult result)
        {
            if (client == null)
                client = new JPushClient(appKey, masterSecret);
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.ios();
            pushPayload.audience = Audience.all();
            var notification = new Notification();
            #region Android
            var androidNotification = new AndroidNotification();
            androidNotification.setAlert(Msg);
            androidNotification.setTitle(Title);
            notification.AndroidNotification = androidNotification;
            #endregion
            pushPayload.notification = notification;
            pushPayload.message = Message.content(Msg);
            result = client.SendPush(pushPayload);
            return true;
        }

        /// <summary>
        /// 个人推送
        /// </summary>
        /// <param name="RegIds">个人注册ID集</param>
        /// <param name="Alert">广播行为</param>
        /// <param name="Msg">自定义消息</param>
        /// <param name="result">回调数据</param>
        /// <returns></returns>
        public bool PushPersonal(string[] RegIds, string Title, string Content, string Msg, ref MessageResult result)
        {
            try
            {
                bool ResetOptionsApnsProduction = true;
                bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["RESETOPTIONSAPNSPRODUCTION"].ToString(), out ResetOptionsApnsProduction);

                if (client == null)
                    client = new JPushClient(appKey, masterSecret);
                PushPayload pushPayload = new PushPayload();
                pushPayload.platform = Platform.all();
                pushPayload.audience = Audience.s_registrationId(RegIds);
                var notification = new Notification();

                #region Android
                var androidNotification = new AndroidNotification();
                androidNotification.setAlert(Content);
                androidNotification.setTitle(Title);
                notification.AndroidNotification = androidNotification;
                #endregion

                #region IOS
                var iosNotification = new IosNotification();
                iosNotification.setAlert(Content);
                iosNotification.setBadge(1);
                iosNotification.setSound("default");
                notification.IosNotification = iosNotification;
                #endregion

                pushPayload.notification = notification;
                pushPayload.ResetOptionsApnsProduction(ResetOptionsApnsProduction);
                if (!string.IsNullOrEmpty(Msg))
                    pushPayload.message = Message.content(Msg);
                result = client.SendPush(pushPayload);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("个人推送错误[BroadcastNotice]：{0}", ex.Message));
            }
        }



    }
}
