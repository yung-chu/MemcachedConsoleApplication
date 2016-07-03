
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LTISP.ACL.Cache.Memcached
{
    public class AspxCache : ICached
    {
        public static ICached Cached = new AspxCache();

        private AspxCache()
        {
        }

        public bool InitCacheData()
        {
            return true;
        }

        public bool CacheDel(string pKey)
        {
            HttpRuntime.Cache.Remove(pKey);
            return true;
        }

        public object CacheGet(string pKey)
        {
            return HttpRuntime.Cache.Get(pKey);
        }

        public object[] CacheGetMulti(string[] pKeys)
        {
            List<object> list = new List<object>();
            foreach (var pk in pKeys)
            {
                var obj = HttpRuntime.Cache.Get(pk);
                if (obj != null)
                    list.Add(obj);
            }
            return list.ToArray();
        }

        public bool CacheSet(string pKey, object oVal, double expirySecs)
        {
            HttpRuntime.Cache.Insert(pKey, oVal, null,
                DateTime.Now.AddSeconds(expirySecs), System.Web.Caching.Cache.NoSlidingExpiration);
            return true;
        }

        public bool CacheSet(string pKey, object oVal)
        {
            HttpRuntime.Cache.Insert(pKey, oVal, null,
                DateTime.Now.AddSeconds(600), System.Web.Caching.Cache.NoSlidingExpiration);
            return true;
        }

        public bool CacheSet(string pKey, object oVal, DateTime expiry)
        {
            HttpRuntime.Cache.Insert(pKey, oVal, null,
                expiry, System.Web.Caching.Cache.NoSlidingExpiration);
            return true;
        }
    }
}
