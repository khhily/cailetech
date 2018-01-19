using CL.Entity.Json.WebAPI;
using CL.Enum.Common;
using CL.Game.Entity;
using CL.Json.Entity.WebAPI;
using CL.Tools.RedisBase;
using CL.View.Entity.Game;
using CL.View.Entity.Other;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CL.Redis.BLL
{
    public class BusinessRedis
    {
        #region ---------- 当前开期 开奖列表 方案记录----------
        /// <summary>
        /// 读取Redis数据
        /// 当期开期数据
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public udv_IsuseInfo CurrentIsuseRedis(int LotteryCode)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.CurrentIsuse, LotteryCode);
            return RedisHelper.Get_Entity<udv_IsuseInfo>(Key);
        }
        /// <summary>
        /// 插入数据Redis
        /// 当期开期数据
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="Minutes"></param>
        /// <returns></returns>
        public bool CurrentIsuseRedis(udv_IsuseInfo Entity, DateTime EndTime)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.CurrentIsuse, Entity.LotteryCode);
            var result = RedisHelper.Set_Entity<udv_IsuseInfo>(Key, Entity);
            RedisHelper.SetExpire_List(Key, EndTime);
            return result;
        }

        /// <summary>
        /// 读取Redis数据
        /// 开奖列表
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public List<OfLotteryEntity> OfLotteryRecordRedis(int LotteryCode, int PageIndex, int PageSize)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.OfLottery, LotteryCode);
            return RedisHelper.GetList_SortedSet<OfLotteryEntity>(Key, PageIndex, PageSize);
        }
        /// <summary>
        /// 插入数据Redis
        /// 开奖列表
        /// 开奖完成把开奖数据插入到redis中
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="Minutes"></param>
        /// <returns></returns>
        public bool OfLotteryRecordRedis(OfLotteryEntity Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.OfLottery, Entity.LotteryCode);
            return RedisHelper.SortedSet_Add(Key, Entity, Convert.ToInt64(Entity.IsuseNum));
        }

        /// <summary>
        /// 插入数据Redis
        /// 开奖列表
        /// 开奖完成把开奖数据插入到redis中
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="Minutes"></param>
        /// <returns></returns>
        public bool OfLotteryDetailRedis(OfLotteryDetailEntity Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.OfLotteryDetail, Entity.LotteryCode);
            string DataKey = string.Format("{0}", Entity.IsuseNum);
            return RedisHelper.Hash_Set<OfLotteryDetailEntity>(Key, DataKey, Entity);
        }
        /// <summary>
        /// 插入数据Redis
        /// 开奖列表
        /// 开奖完成把开奖数据插入到redis中
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="Minutes"></param>
        /// <returns></returns>
        public OfLotteryDetailEntity OfLotteryDetailRedis(int LotteryCode, string IsuseNum)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.OfLotteryDetail, LotteryCode);
            string DataKey = string.Format("{0}", IsuseNum);
            return RedisHelper.Hash_Get<OfLotteryDetailEntity>(Key, DataKey);
        }
        /// <summary>
        /// Redis删除:手动开奖数据 低频彩
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="IsuseName"></param>
        /// <returns></returns>
        public bool RemoveOfLotteryDetailRedis(int LotteryCode, string IsuseName)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.OfLotteryDetail, LotteryCode);
            return RedisHelper.Remove_Entity(Key);
        }

        /// <summary>
        /// 插入redis数据
        /// 方案详情
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="SchemeID"></param>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool SchemeDetailRedis(long UserCode, long SchemeID, SchemeDetailEntity Entity)
        {
            string HashKey = string.Format("{0}:{1}", RedisKeysEnum.SchemeDetail, UserCode);
            string Key = string.Format("{0}", SchemeID);
            return RedisHelper.Hash_Set(HashKey, Key, Entity);
        }
        /// <summary>
        /// 查询redis数据
        /// 方案详情
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public SchemeDetailEntity SchemeDetailRedis(long UserCode, long SchemeID)
        {
            string HashKey = string.Format("{0}:{1}", RedisKeysEnum.SchemeDetail, UserCode);
            string Key = string.Format("{0}", SchemeID);
            return RedisHelper.Hash_Get<SchemeDetailEntity>(HashKey, Key);
        }

        #endregion

        #region ---------- 追号记录 追号详情 ----------
        /// <summary>
        /// 插入redis数据(不完整)
        /// 追号记录
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool ChaseRecordRedis(long UserCode, long SchemeID, ChaseRecordEntity Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.ChaseRecord, UserCode);
            return RedisHelper.Hash_Set(Key, SchemeID.ToString(), Entity);
        }
        /// <summary>
        /// 读取Redis数据
        /// 追号记录
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        public ChaseRecordEntity ChaseRecordRedis(long UserCode, long SchemeID)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.ChaseRecord, UserCode);
            return RedisHelper.Hash_Get<ChaseRecordEntity>(Key, SchemeID.ToString());
        }
        /// <summary>
        /// 插入Redis数据
        /// 追号详情
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="ChaseDetailCode"></param>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool ChaseDetailRedis(long UserCode, long ChaseDetailCode, ChaseDetailEntity Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.ChaseRecordDetail, UserCode);
            return RedisHelper.Hash_Set(Key, ChaseDetailCode.ToString(), Entity);
        }
        /// <summary>
        /// 读取Redis数据
        /// 追号详情
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="ChaseDetailCode"></param>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public ChaseDetailEntity ChaseDetailRedis(long UserCode, long ChaseDetailCode)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.ChaseRecordDetail, UserCode);
            return RedisHelper.Hash_Get<ChaseDetailEntity>(Key, ChaseDetailCode.ToString());
        }



        #endregion

        #region ---------- 手动开奖 ----------
        /// <summary>
        /// Redis查询:手动开奖数据
        /// 五分钟过期时间
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="IsuseName"></param>
        /// <returns></returns>
        public udv_ManualOpenLottery ManualOpenLotteryRedis(int LotteryCode, string IsuseName)
        {
            string Key = string.Format("{0}:{1}:{2}", RedisKeysEnum.ManualOpenLottery, LotteryCode, IsuseName);
            return RedisHelper.Get_Entity<udv_ManualOpenLottery>(Key);
        }
        /// <summary>
        /// Redis插入:手动开奖数据
        /// 五分钟过期时间
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool ManualOpenLotteryRedis(udv_ManualOpenLottery Entity)
        {
            string Key = string.Format("{0}:{1}:{2}", RedisKeysEnum.ManualOpenLottery, Entity.LotteryCode, Entity.IsuseName);
            return RedisHelper.Set_Entity(Key, Entity, 0, 5, 0);
        }
        /// <summary>
        /// Redis删除:手动开奖数据 高频彩
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="IsuseName"></param>
        /// <returns></returns>
        public bool RemoveManualOpenLotteryRedis(int LotteryCode, string IsuseName)
        {
            string Key = string.Format("{0}:{1}:{2}", RedisKeysEnum.ManualOpenLottery, LotteryCode, IsuseName);
            return RedisHelper.Remove_Entity(Key);
        }
        #endregion


        #region 走势图缓存数据
        /// <summary>
        /// 存储Redis：号码走势图基础数据查询
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="IsuseName"></param>
        /// <param name="Entitys"></param>
        /// <returns></returns>
        public bool LotTrendChartRedis(int LotteryCode, string IsuseName, List<IsusesEntity> Entitys, int TOPCount)
        {
            string LotKey = string.Format("{0}:{1}:{2}", RedisKeysEnum.Lot, LotteryCode, TOPCount);
            //string IssuKey = string.Format("{0}:{1}", RedisKeysEnum.IsuseName, LotteryCode);
            //RedisHelper.Set_Entity<string>(IssuKey, IsuseName);
            bool rec = RedisHelper.Set_Entity(LotKey, Entitys);
            RedisHelper.SetExpire_List(LotKey, DateTime.Now.AddMinutes(1));
            return rec;
        }
        /// <summary>
        /// 查询Redis：号码走势图基础数据查询
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="IsuseName"></param>
        /// <returns></returns>
        public List<IsusesEntity> LotTrendChartRedis(int LotteryCode, string IsuseName, int TOPCount)
        {
            string LotKey = string.Format("{0}:{1}:{2}", RedisKeysEnum.Lot, LotteryCode, TOPCount);
            //string IssuKey = string.Format("{0}:{1}", RedisKeysEnum.IsuseName, LotteryCode);
            //var IsuseNameRedis = RedisHelper.Get_Entity<string>(IssuKey);
            //if (IsuseNameRedis != IsuseName)
            //    return null;
            //else
            return RedisHelper.Get_Entity<List<IsusesEntity>>(LotKey);
        }
        #endregion

        #region 数据结构对象
        #region 期次号
        /// <summary>
        /// 查询Redis期次编号查询期次号
        /// </summary>
        /// <param name="IsusesID"></param>
        /// <returns></returns>
        public string QueryIssueNameRedis(long IsusesID)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.IsusesName, IsusesID);
            return RedisHelper.Get_String<string>(Key);
        }
        /// <summary>
        /// 保存Redis期次编号
        /// </summary>
        /// <param name="IsusesID"></param>
        /// <param name="IsuseName"></param>
        /// <returns></returns>
        public bool IssueNameRedis(long IsusesID, string IsuseName)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.IsusesName, IsusesID);
            return RedisHelper.Set_String(Key, IsuseName);
        }
        #endregion

        #region 模版
        /// <summary>
        /// 查询Redis模版数据
        /// </summary>
        /// <param name="TemplateType"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public TemplateConfigEntity QueryTemplateConfigEntityRedis(byte TemplateType, int ID)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.TemplateConfig, TemplateType);
            return RedisHelper.Hash_Get<TemplateConfigEntity>(Key, ID.ToString());
        }

        /// <summary>
        /// 存储Redis模版数据
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool TemplateConfigEntityRedis(TemplateConfigEntity Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.TemplateConfig, Entity.TemplateType);
            return RedisHelper.Hash_Set(Key, Entity.ID.ToString(), Entity);
        }
        public List<TemplateConfigEntity> QueryTemplateConfigEntityRedis(byte TemplateType)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.TemplateConfig, TemplateType);
            return RedisHelper.Hash_GetAll<TemplateConfigEntity>(Key);
        }


        #endregion
        #endregion

        #region 新的方案存储方式
        /// <summary>
        /// 保存方案中奖记录
        /// 列表最大存储33行
        /// 存储三页  每页11行
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool Save_SchemesWinRedis(long UserCode, SchemeRecordEntity Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.SchemeWin, UserCode);
            string Hash_Key = Entity.OrderCode.ToString();
            long OrderCode = 0; //最前的订单 值最小的
            int HashLeng = Query_HashAllLeng(Key, ref OrderCode);
            if (HashLeng >= 33)
                RedisHelper.Hash_Remove(Key, OrderCode.ToString());
            return RedisHelper.Hash_Set(Key, Hash_Key, Entity);
        }

        /// <summary>
        /// 查询方案中奖记录
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public List<SchemeRecordEntity> Query_SchemesWinRedis(long UserCode, int PageIndex, int PageSize)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.SchemeWin, UserCode);
            var Entitys = RedisHelper.Hash_GetAll<SchemeRecordEntity>(Key);
            if (Entitys == null || Entitys.Count == 0)
                return null;
            else
            {
                Entitys = Entitys.OrderByDescending(o => o.OrderCode).ToList();
                return Entitys.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
        }
        /// <summary>
        /// 保存方案待开奖记录
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool Save_SchemeWaitRedis(long UserCode, SchemeRecordEntity Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.SchemeWait, UserCode);
            string Hash_Key = Entity.OrderCode.ToString();
            return RedisHelper.Hash_Set(Key, Hash_Key, Entity);
        }
        /// <summary>
        /// 查询方案待开奖记录
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public List<SchemeRecordEntity> Query_SchemeWaitRedis(long UserCode, int PageIndex, int PageSize)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.SchemeWait, UserCode);
            var Entitys = RedisHelper.Hash_GetAll<SchemeRecordEntity>(Key);
            if (Entitys == null || Entitys.Count == 0)
                return null;
            else
            {
                Entitys = Entitys.OrderByDescending(o => o.OrderCode).ToList();
                return Entitys.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
        }
        /// <summary>
        /// 查询方案待开奖记录
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public SchemeRecordEntity Query_SchemeWaitRedis(long UserCode, long OrderCode)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.SchemeWait, UserCode);
            return RedisHelper.Hash_Get<SchemeRecordEntity>(Key, OrderCode.ToString());
        }
        /// <summary>
        /// 删除待开奖记录
        /// 待开奖记录在开奖之后必须删除
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="OrderCode"></param>
        /// <returns></returns>
        public bool Remove_SchemeWaitRedis(long UserCode, long OrderCode)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.SchemeWait, UserCode);
            string Hash_Key = OrderCode.ToString();
            return RedisHelper.Hash_Remove(Key, Hash_Key);
        }
        /// <summary>
        /// 保存全部方案记录
        /// 列表最大存储33行
        /// 存储三页  每页11行
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool Save_SchemeRedis(long UserCode, SchemeRecordEntity Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.Scheme, UserCode);
            string Hash_Key = Entity.OrderCode.ToString();
            long OrderCode = 0; //最前的订单 值最小的
            int HashLeng = Query_HashAllLeng(Key, ref OrderCode);
            if (HashLeng >= 33)
                RedisHelper.Hash_Remove(Key, OrderCode.ToString());
            return RedisHelper.Hash_Set(Key, Hash_Key, Entity);
        }
        /// <summary>
        /// 更新全部方案记录
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool Update_SchemeRedis(long UserCode, SchemeRecordEntity Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.Scheme, UserCode);
            string Hash_Key = Entity.OrderCode.ToString();
            return RedisHelper.Hash_Set(Key, Hash_Key, Entity);
        }
        /// <summary>
        /// 查询全部方案记录
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public List<SchemeRecordEntity> Query_SchemeRedis(long UserCode, int PageIndex, int PageSize)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.Scheme, UserCode);
            var Entitys = RedisHelper.Hash_GetAll<SchemeRecordEntity>(Key);
            if (Entitys == null || Entitys.Count == 0)
                return null;
            else
            {
                Entitys = Entitys.OrderByDescending(o => o.OrderCode).ToList();
                return Entitys.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
        }
        /// <summary>
        /// 查询全部方案记录
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public SchemeRecordEntity Query_SchemeRedis(long UserCode, long OrderCode)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.Scheme, UserCode);
            string Hash_Key = OrderCode.ToString();
            return RedisHelper.Hash_Get<SchemeRecordEntity>(Key, Hash_Key);
        }
        /// <summary>
        /// 保存方案追号记录
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool Save_SchemeChase(long UserCode, SchemeRecordEntity Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.SchemeChase, UserCode);
            string Hash_Key = Entity.OrderCode.ToString();
            long OrderCode = 0; //最前的订单 值最小的
            int HashLeng = Query_HashAllLeng(Key, ref OrderCode);
            if (HashLeng >= 33)
                RedisHelper.Hash_Remove(Key, OrderCode.ToString());
            return RedisHelper.Hash_Set(Key, Hash_Key, Entity);
        }
        /// <summary>
        /// 更新方案追号记录
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool Update_SchemeChase(long UserCode, SchemeRecordEntity Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.SchemeChase, UserCode);
            string Hash_Key = Entity.OrderCode.ToString();
            return RedisHelper.Hash_Set(Key, Hash_Key, Entity);
        }
        /// <summary>
        /// 查询方案追号记录
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="OrderCode"></param>
        /// <returns></returns>
        public SchemeRecordEntity Query_SchemeChase(long UserCode, long OrderCode)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.SchemeChase, UserCode);
            string Hash_Key = OrderCode.ToString();
            return RedisHelper.Hash_Get<SchemeRecordEntity>(Key, Hash_Key);
        }
        /// <summary>
        /// 查询方案追号记录
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public List<SchemeRecordEntity> Query_SchemeChase(long UserCode, int PageIndex, int PageSize)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.SchemeChase, UserCode);
            var Entitys = RedisHelper.Hash_GetAll<SchemeRecordEntity>(Key);
            if (Entitys == null || Entitys.Count == 0)
                return null;
            else
            {
                Entitys = Entitys.OrderByDescending(o => o.OrderCode).ToList();
                return Entitys.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
        }



        /// <summary>
        /// 哈希长度
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="LastOrderCode"></param>
        /// <returns></returns>
        int Query_HashAllLeng(string Key, ref long LastOrderCode)
        {
            var Entitys = RedisHelper.Hash_GetAll<SchemeRecordEntity>(Key);
            if (Entitys == null || Entitys.Count == 0)
                return 0;
            else
            {
                LastOrderCode = Entitys.OrderBy(o => o.OrderCode).FirstOrDefault().OrderCode;
                return Entitys.Count;
            }
        }

        #endregion

        #region 开奖大厅
        /// <summary>
        /// 保存开奖大厅
        /// </summary>
        /// <param name="Entitys"></param>
        /// <returns></returns>
        public bool Save_OpenAwardHall(List<IsusesEntity> Entitys)
        {
            string Key = string.Format("Apply:{0}", RedisKeysEnum.OpenAwardHall.ToString());
            return RedisHelper.Set_Entity(Key, Entitys, 0, 1, 0);
        }
        /// <summary>
        /// 读取开奖大厅
        /// </summary>
        /// <returns></returns>
        public List<IsusesEntity> Query_OpenAwardHall()
        {
            string Key = string.Format("Apply:{0}", RedisKeysEnum.OpenAwardHall.ToString());
            return RedisHelper.Get_Entity<List<IsusesEntity>>(Key);
        }

        #endregion

        #region Http代理数据缓存
        /// <summary>
        /// 插入代理对象集
        /// </summary>
        /// <param name="Entitys"></param>
        /// <returns></returns>
        public bool InsertHttpProxy(List<udv_HttpProxy> Entitys)
        {
            string Key = string.Format("Http:{0}", RedisKeysEnum.Proxy);
            return RedisHelper.Set_Entity(Key, Entitys, 0, 10, 0);
        }
        /// <summary>
        /// 获取代理对象集
        /// </summary>
        /// <returns></returns>
        public List<udv_HttpProxy> QueryHttpProxy()
        {
            string Key = string.Format("Http:{0}", RedisKeysEnum.Proxy);
            return RedisHelper.Get_Entity<List<udv_HttpProxy>>(Key);
        }
        #endregion

        #region 新闻资讯
        /// <summary>
        /// 保存分析资讯标题
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="Entitys"></param>
        /// <returns></returns>
        public bool SaveLotAnalysisRedis(int LotteryCode, List<NewsTitle> Entitys)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.NewAnalysis, LotteryCode);
            return RedisHelper.Set_Entity(Key, Entitys, 0, 30, 0);
        }
        /// <summary>
        /// 读取分析资讯标题
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<NewsTitle> QueryLotAnalysisRedis(int LotteryCode)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.NewAnalysis, LotteryCode);
            return RedisHelper.Get_Entity<List<NewsTitle>>(Key);
        }
        /// <summary>
        /// 保存客户端资讯
        /// </summary>
        /// <param name="NewsID"></param>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool SaveClientNewsRedis(int NewsID, ClientNews Entity)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.ClientNews, NewsID);
            return RedisHelper.Set_Entity(Key, Entity, 0, 10, 0);
        }
        /// <summary>
        /// 读取分析资讯标题
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public ClientNews QueryClientNewsRedis(int NewsID)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.ClientNews, NewsID);
            return RedisHelper.Get_Entity<ClientNews>(Key);
        }
        #endregion

        #region 房间
        /// <summary>
        /// 保存房间信息
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="Entitys"></param>
        /// <returns></returns>
        public bool SaveRooms(int LotteryCode, List<Room> Entitys)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.Rooms, LotteryCode);
            return RedisHelper.Set_Entity(Key, Entitys);
        }
        /// <summary>
        /// 查询房间信息
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<Room> QueryRooms(int LotteryCode)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.Rooms, LotteryCode);
            return RedisHelper.Get_Entity<List<Room>>(Key);
        }
        /// <summary>
        /// 移除房间信息
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public bool RemoveRooms(int LotteryCode)
        {
            string Key = string.Format("{0}:{1}", RedisKeysEnum.Rooms, LotteryCode);
            return RedisHelper.Remove_Entity(Key);
        }
        #endregion
    }

}
