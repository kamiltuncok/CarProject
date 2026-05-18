using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace Core.CrossCuttingConcerns.Microsoft
{
    public class MemoryCacheManager : ICacheManager
    {
        IMemoryCache _memoryCache;
    public MemoryCacheManager()
    {
        _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
    }
    public void Add(string key, object value, int duration)
    {
        _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));
    }

    public T Get<T>(string key)
    {
        return _memoryCache.Get<T>(key);
    }

    public object Get(string key)
    {
        return _memoryCache.Get(key);
    }

    public bool IsAdd(string key)
    {
        return _memoryCache.TryGetValue(key, out _);
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }

    public void RemoveByPattern(string pattern)
    {
        try
        {
            var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (cacheEntriesCollectionDefinition == null)
            {
                // In .NET 7/8/9, EntriesCollection is not there. Try modern reflection:
                var coherentStateField = typeof(MemoryCache).GetField("_coherentState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (coherentStateField != null)
                {
                    var coherentState = coherentStateField.GetValue(_memoryCache);
                    if (coherentState != null)
                    {
                        var entriesField = coherentState.GetType().GetField("_entries", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (entriesField != null)
                        {
                            var entries = entriesField.GetValue(coherentState) as System.Collections.IDictionary;
                            if (entries != null)
                            {
                                var keysToRemove = new List<object>();
                                var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                foreach (var key in entries.Keys)
                                {
                                    if (regex.IsMatch(key.ToString()))
                                    {
                                        keysToRemove.Add(key);
                                    }
                                }
                                foreach (var key in keysToRemove)
                                {
                                    _memoryCache.Remove(key);
                                }
                                return;
                            }
                        }
                    }
                }
                
                // Fallback: If reflection completely fails, prevent crash
                return;
            }

            var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_memoryCache) as dynamic;
            if (cacheEntriesCollection == null) return;

            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();

            foreach (var cacheItem in cacheEntriesCollection)
            {
                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(cacheItemValue);
            }

            var regexOld = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemoveOld = cacheCollectionValues.Where(d => regexOld.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();

            foreach (var key in keysToRemoveOld)
            {
                _memoryCache.Remove(key);
            }
        }
        catch (Exception)
        {
            // Fail silently to prevent crashing core application logic due to cache reflection failure
        }
    }
}
}
