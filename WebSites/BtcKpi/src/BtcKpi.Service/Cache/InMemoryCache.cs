﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BtcKpi.Service.Cache
{
    //public class InMemoryCache : ICacheService
    //{
    //    public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class
    //    {
    //        T item = MemoryCache.Default.Get(cacheKey) as T;
    //        if (item == null)
    //        {
    //            item = getItemCallback();
    //            MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddMinutes(10));
    //        }
    //        return item;
    //    }
    //}

    //interface ICacheService
    //{
    //    T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class;
    //}
}
