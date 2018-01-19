using ServiceStack.Redis;
using System;
using System.Collections.Generic;

namespace CL.Tools.RedisBase
{
    public class RedisHelper
    {
        #region -- Entity --
        /// <summary>
        /// 设置单个实体
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="key">键</param>
        /// <param name="t">值</param>
        /// <param name="timeSpan">时间</param>
        /// <returns></returns>
        public static bool Set_Entity<T>(string key, T t, int Hours = 0, int Minutes = 0, int Seconds = 0)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    if (Hours == 0 && Minutes == 0 && Seconds == 0)
                        return redisClient.Set<T>(key, t);
                    else
                        return redisClient.Set<T>(key, t, new TimeSpan(Hours, Minutes, Seconds));
                }
            }
            catch 
            {
                return false;
            }
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get_Entity<T>(string key) where T : class
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    return redisClient.Get<T>(key);
                }
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// 移除单体
        /// </summary>
        /// <param name="key"></param>
        public static bool Remove_Entity(string key)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    return redisClient.Remove(key);
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region -- String --

        /// <summary>
        /// 设置字符串
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="str">值</param>
        /// <param name="timeSpan">时间</param>
        /// <returns></returns>
        public static bool Set_String(string key, string str, int Hours = 0, int minutes = 0, int seconds = 0)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    if (Hours == 0 && minutes == 0 && seconds == 0)
                        return redisClient.Set<string>(key, str);
                    return redisClient.Set<string>(key, str, new TimeSpan(Hours, minutes, seconds));
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get_String<T>(string key)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    return redisClient.Get<string>(key);
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region -- List --
        /// <summary>
        /// 设置列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        public static void Set_List<T>(string key, T t)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    var redisTypedClient = redisClient.GetTypedClient<T>();
                    redisTypedClient.AddItemToList(redisTypedClient.Lists[key], t);
                }
            }
            catch
            {
                return;
            }
        }
        public static bool Remove_List<T>(string key, T t)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    var redisTypedClient = redisClient.GetTypedClient<T>();
                    return redisTypedClient.RemoveItemFromList(redisTypedClient.Lists[key], t) > 0;
                }
            }
            catch
            {
                return false;
            }
        }
        public static void RemoveAll_List<T>(string key)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    var redisTypedClient = redisClient.GetTypedClient<T>();
                    redisTypedClient.Lists[key].RemoveAll();
                }
            }
            catch
            {
                return;
            }
        }
        public static long Count_List(string key)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    return redisClient.GetListCount(key);
                }
            }
            catch
            {
                return 0;
            }
        }
        public static List<T> GetRange_List<T>(string key, int start, int count)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    var c = redisClient.GetTypedClient<T>();
                    return c.Lists[key].GetRange(start, start + count - 1);
                }
            }
            catch
            {
                return null;
            }
        }

        public static List<T> GetList_List<T>(string key)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    var c = redisClient.GetTypedClient<T>();
                    return c.Lists[key].GetRange(0, c.Lists[key].Count);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public static void SetExpire_List(string key, DateTime datetime)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    redisClient.ExpireEntryAt(key, datetime);
                }
            }
            catch
            {
                return;
            }
        }
        #endregion

        #region -- HashSet --
        public static void Set_Add<T>(string key, T t)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    var redisTypedClient = redisClient.GetTypedClient<T>();
                    redisTypedClient.Sets[key].Add(t);
                }
            }
            catch
            {
                return;
            }
        }
        public static bool Set_Contains<T>(string key, T t)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    var redisTypedClient = redisClient.GetTypedClient<T>();
                    return redisTypedClient.Sets[key].Contains(t);
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool Set_Remove<T>(string key, T t)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    var redisTypedClient = redisClient.GetTypedClient<T>();
                    return redisTypedClient.Sets[key].Remove(t);
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region -- Hash --
        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool Hash_Exist<T>(string key, string dataKey)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    return redisClient.HashContainsEntry(key, dataKey);
                }
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool Hash_Set<T>(string key, string dataKey, T t)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                    return redisClient.SetEntryInHash(key, dataKey, value);
                }
            }
            catch
            {
                return false;
            }
        }



        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool Hash_Remove(string key, string dataKey)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    return redisClient.RemoveEntryFromHash(key, dataKey);
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 移除整个hash
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool Hash_Remove(string key)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    return redisClient.Remove(key);
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static T Hash_Get<T>(string key, string dataKey)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    string value = redisClient.GetValueFromHash(key, dataKey);
                    return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(value);
                }
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 获取整个hash的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> Hash_GetAll<T>(string key)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    
                    var list = redisClient.GetHashValues(key);
                    if (list != null && list.Count > 0)
                    {
                        List<T> result = new List<T>();
                        foreach (var item in list)
                        {
                            var value = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                            result.Add(value);
                        }
                        return result;
                    }
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        ///// <summary>
        ///// 获取整个hash的数据
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static List<T> Hash_GetAll_AcquireLock<T>(string key)
        //{
        //    try
        //    {

        //        using (var redisClient = RedisManager.GetClient())
        //        {
        //            using (redisClient.AcquireLock(key, new TimeSpan(0, 0, 5)))
        //            {
        //                var list = redisClient.GetHashValues(key);
        //                if (list != null && list.Count > 0)
        //                {
        //                    List<T> result = new List<T>();
        //                    foreach (var item in list)
        //                    {
        //                        var value = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
        //                        result.Add(value);
        //                    }
        //                    return result;
        //                }
        //            }
        //            return null;
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public static void Hash_SetExpire(string key, DateTime datetime)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    redisClient.ExpireEntryAt(key, datetime);
                }
            }
            catch
            {
                return;
            }
        }
        #endregion

        #region -- SortedSet --
        /// <summary>
        ///  添加数据到 SortedSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="score"></param>
        public static bool SortedSet_Add<T>(string key, T t, double score)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                    return redisClient.AddItemToSortedSet(key, value, score);
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 移除数据从SortedSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Remove_SortedSet<T>(string key, T t)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                    return redisClient.RemoveItemFromSortedSet(key, value);
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 修剪SortedSet
        /// </summary>
        /// <param name="key"></param>
        /// <param name="size">保留的条数</param>
        /// <returns></returns>
        public static long SortedSet_Trim(string key, int size)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    return redisClient.RemoveRangeFromSortedSet(key, size, 9999999);
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 移除指定的Score
        /// </summary>
        /// <param name="key"></param>
        /// <param name="Score"></param>
        /// <returns></returns>
        public static long SortedSet_TrimByScore(string key, long Score)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    return redisClient.RemoveRangeFromSortedSetByScore(key, Score, Score);
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 获取SortedSet的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long Count_SortedSet(string key)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    return redisClient.GetSortedSetCount(key);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 获取SortedSet的分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> GetList_SortedSet<T>(string key, int pageIndex, int pageSize)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    //var list = redisClient.GetRangeFromSortedSet(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1);
                    var list = redisClient.GetRangeFromSortedSetDesc(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1);
                    if (list != null && list.Count > 0)
                    {
                        List<T> result = new List<T>();
                        foreach (var item in list)
                        {
                            var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                            result.Add(data);
                        }
                        return result;
                    }
                }
                return null;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 获取SortedSet的全部数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> GetListALL_SortedSet<T>(string key)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    var list = redisClient.GetRangeFromSortedSet(key, 0, 9999999);
                    if (list != null && list.Count > 0)
                    {
                        List<T> result = new List<T>();
                        foreach (var item in list)
                        {
                            var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                            result.Add(data);
                        }
                        return result;
                    }
                }
                return null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public static void SetExpire_SortedSet(string key, DateTime datetime)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    redisClient.ExpireEntryAt(key, datetime);
                }
            }
            catch
            {
                throw;
            }
        }

        public static double GetItemScore_SortedSet<T>(string key, T t)
        {
            try
            {
                using (var redisClient = RedisManager.GetClient())
                {
                    var data = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                    return redisClient.GetItemScoreInSortedSet(key, data);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
