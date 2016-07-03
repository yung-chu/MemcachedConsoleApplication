using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LTISP.ACL.Cache.Memcached
{
    public interface ICached
    {
        #region 初始化缓存

        bool InitCacheData();

        #endregion

        #region Cache Delete
        bool CacheDel(string pKey);

        #endregion

        #region Cache Get
        object CacheGet(string pKey);
        object[] CacheGetMulti(string[] pKeys);

        #endregion

        #region Cache Set
        bool CacheSet(string pKey, object oVal, double expirySecs);
        bool CacheSet(string pKey, object oVal);
        bool CacheSet(string pKey, object oVal, DateTime expiry);

        #endregion

    }
}
