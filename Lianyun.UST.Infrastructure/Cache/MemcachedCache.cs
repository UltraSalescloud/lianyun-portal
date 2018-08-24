using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enyim.Caching;
using Lianyun.UST.Infrastructure.Properties;
using Enyim.Caching.Memcached;
using System.Web.Caching;

namespace Lianyun.UST.Infrastructure.Cache
{
    public class MemcachedCache : ICache
    {
        private MemcachedClient cache;
        
        private TimeSpan _timeSpan = new TimeSpan(
            Settings.Default.DefaultCacheDuration_Days,
            Settings.Default.DefaultCacheDuration_Hours,
            Settings.Default.DefaultCacheDuration_Minutes, 0);

        public MemcachedCache()
        {
            cache = new MemcachedClient();

            List<string> keys = new List<string>();
            cache.Store(StoreMode.Add, "keys", keys);
        }

        public object Get(string cache_key)
        {
            return cache.Get(cache_key);
        }

        public List<string> GetCacheKeys()
        {
            return cache.Get("keys") as List<string>;
        }
              
        public void Set(string cache_key, object cache_object)
        {
            Set(cache_key, cache_object, _timeSpan);
        }
     
        public void Set(string cache_key, object cache_object, DateTime expiration)
        {
            Set(cache_key, cache_object, expiration, CacheItemPriority.Normal);
        }

     
        public void Set(string cache_key, object cache_object, TimeSpan expiration)
        {
            Set(cache_key, cache_object, expiration, CacheItemPriority.Normal);
        }

      
        public void Set(string cache_key, object cache_object, DateTime expiration, CacheItemPriority priority)
        {
            cache.Store(StoreMode.Set, cache_key, cache_object, expiration);
            UpdateKeys(cache_key);
        }

    
        public void Set(string cache_key, object cache_object, TimeSpan expiration, CacheItemPriority priority)
        {
            cache.Store(StoreMode.Set, cache_key, cache_object, expiration);
            UpdateKeys(cache_key);
        }

        private void UpdateKeys(string key)
        {
            List<string> keys = new List<string>();
            if (cache.Get("keys") != null)
            {
                keys = cache.Get("keys") as List<string>;
            }

            if (!keys.Contains(key.ToLower()))
            {
                keys.Add(key);
                cache.Store(StoreMode.Set, "keys", keys);
            }
        }

      
        public void Delete(string cache_key)
        {
            if (Exists(cache_key))
                cache.Remove(cache_key);
        }

       
        public bool Exists(string cache_key)
        {
            if (cache.Get(cache_key) != null)
                return true;
            else
                return false;
        }
      
        public void Flush()
        {
            cache.FlushAll();
        }
    }
}
