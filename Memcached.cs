using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Memcached.ClientLibrary;

using System.Configuration;

namespace LTISP.ACL.Cache.Memcached
{
    /// <summary>
    /// 缓存的Memcached实现
    /// </summary>
    public class MemcachedITS : ICached
    {
        private MemcachedClient mc = null;
        private SockIOPool pool = null;
        private string[] ServerList = { "127.0.0.1:11211" };
        private int MaxConns = 10;
        private bool EnableCompression = false;
        private DateTime ExpiryMax;

        /// <summary>
        /// 获取Memcached的单实例
        /// </summary>
        public static ICached Cached = new MemcachedITS();

        private MemcachedITS()
        {
            ExpiryMax = DateTime.Now.AddYears(10);
            InitConfigSetting();
            InitMemcahced();
        }
        private bool InitMemcahced()
        {
            if (pool == null)
            {
                try
                {


                    //int[] weights = new int[] { 5, 2 }; 

                    //初始化池
                    pool = SockIOPool.GetInstance();
                    pool.SetServers(ServerList);

                    //pool.SetWeights(weights);　方法就是设配各个服务器负载作用的系数，系数值越大，其负载作用也就越大


                    pool.InitConnections = 3;
                    pool.MinConnections = 3;
                    pool.MaxConnections = MaxConns;

                    pool.SocketConnectTimeout = 1000;
                    pool.SocketTimeout = 3000;

                    pool.MaintenanceSleep = 30;
                    pool.Failover = true;

                    pool.Nagle = false;
                    pool.Initialize();
                }
                catch (Exception ex)
                {
                   // Lgr.Log.Error(ex.Message, ex);
                    try
                    {
                        if (pool != null)
                            pool.Shutdown();
                    }
                    catch { }
                    pool = null;
                }
            }
            if (mc == null)
            {
                try
                {
                    // 获得客户端实例
                    mc = new MemcachedClient();
                    mc.EnableCompression = EnableCompression;
                    return true;
                }
                catch (Exception ex)
                {
                    mc = null;
                   // Lgr.Log.Error(ex.Message, ex);
                }
            }
            return pool != null && mc != null;
        }
        private void InitConfigSetting()
        {
            string serverList = ConfigurationManager.AppSettings["MemcachedServerList"];
            string maxConns = ConfigurationManager.AppSettings["MemcachedMaxConns"];
            string compress = ConfigurationManager.AppSettings["MemcachedCompress"];
            if (!string.IsNullOrEmpty(serverList))
                ServerList = serverList.Split(',');
            else
            {
               // Lgr.Log.Info(string.Format("MemcachedServerList 没有配置(多个服务器用逗号分隔)！默认使用{0}", string.Join(",", ServerList)));
            }
            if (!string.IsNullOrEmpty(maxConns))
            {
                if (!int.TryParse(maxConns, out MaxConns))
                {
                   // Lgr.Log.Info(string.Format("MemcachedMaxConns 值[{0}]非法！必须是数值！", maxConns));
                }
            }
            else
            {
               // Lgr.Log.Info(string.Format("MemcachedMaxConns 没有配置！默认使用{0}！", MaxConns));
            }
            if (string.Equals(compress, "1"))
            {
                EnableCompression = true;
              //  Lgr.Log.Info("MemcachedCompress 启用！");
            }
            else
            {
              //  Lgr.Log.Info("MemcachedCompress 禁用！");
            }
        }

        #region 初始化缓存


        public bool InitCacheData()
        {
            return true;
        }

        #endregion


        #region Cache Set

        public bool CacheSet(string pKey, object oVal)
        {
            if (InitMemcahced())
            {
                return mc.Set(pKey, oVal);
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pKey"></param>
        /// <param name="oVal"></param>
        /// <param name="expiry">过期的时间</param>
        /// <returns></returns>
        public bool CacheSet(string pKey, object oVal, DateTime expiry)
        {
            if (InitMemcahced())
            {
                return mc.Set(pKey, oVal, expiry);
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expiry">从现在开始过期的秒数</param>
        /// <returns></returns>
        public bool CacheSet(string pKey, object oVal, double expirySecs)
        {
            if (InitMemcahced())
            {
                DateTime expiry = DateTime.Now.AddSeconds(expirySecs);
                return mc.Set(pKey, oVal, expiry);
            }
            return false;
        }

        #endregion

        #region Cache Get

        public object CacheGet(string pKey)
        {
            if (InitMemcahced())
            {
                return mc.Get(pKey);
            }
            return null;
        }

        public object[] CacheGetMulti(string[] pKeys)
        {
            if (InitMemcahced())
            {
                return mc.GetMultipleArray(pKeys);
            }
            return new object[0];
        }

        #endregion

        #region Cache Delete

        public bool CacheDel(string pKey)
        {
            if (InitMemcahced())
            {
                return mc.Delete(pKey);
            }
            return false;
        }

        #endregion

    }
}
