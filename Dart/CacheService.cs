using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Dart
{
    public interface ICacheService
    {
        void Clear(string key);
        bool ContainsKey(string key);
        string Get(string key);
        T Get<T>(string key);
        void Set<T>(string key, T obj);
        void Set(string key, string obj);
    }

    public class CacheService : ICacheService
    {
        private readonly ILogger<CacheService> _logger;

        public CacheService(ILogger<CacheService> logger)
        {
            _logger = logger;
        }

        public void Clear(string key)
        {
            _logger.LogDebug("Clearing cache key: {Key}", key);
            Preferences.Remove(key);
        }

        public bool ContainsKey(string key)
        {
            var exists = Preferences.ContainsKey(key);
            _logger.LogDebug("Cache key {Key} exists: {Exists}", key, exists);
            return exists;
        }

        public string Get(string key)
        {
            if (!Preferences.ContainsKey(key))
            {
#if DEBUG
                _logger.LogDebug("Cache miss for key: {Key}", key);
#endif
                return "";
            }

            var value = Preferences.Get(key, "").ToString();
#if DEBUG
            _logger.LogDebug("Cache hit for key: {Key}, length: {Length}", key, value?.Length ?? 0);
#endif
            return value;
        }

        public T Get<T>(string key)
        {
            if (!Preferences.ContainsKey(key))
            {
#if DEBUG
                _logger.LogDebug("Cache miss for key: {Key} (type {Type})", key, typeof(T).Name);
#endif
                return default;
            }

            if (string.IsNullOrEmpty(Preferences.Get(key, "")))
            {
#if DEBUG
                _logger.LogDebug("Empty cache value for key: {Key} (type {Type})", key, typeof(T).Name);
#endif
                return default;
            }

            var json = Preferences.Get(key, "").ToString();
            if (string.IsNullOrEmpty(json))
            {
#if DEBUG
                _logger.LogDebug("Empty JSON for cache key: {Key} (type {Type})", key, typeof(T).Name);
#endif
                return default;
            }

            try
            {
                var result = JsonSerializer.Deserialize<T>(json);
#if DEBUG
                _logger.LogDebug("Cache hit for key: {Key} (type {Type})", key, typeof(T).Name);
#endif
                return result;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize cache value for key: {Key} (type {Type})", key, typeof(T).Name);
                return default;
            }
        }

        public void Set<T>(string key, T obj)
        {
            try
            {
                var json = JsonSerializer.Serialize(obj);
                _logger.LogDebug("Setting cache key: {Key} (type {Type}), JSON length: {Length}", key, typeof(T).Name, json.Length);
                Set(key, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to serialize object for cache key: {Key} (type {Type})", key, typeof(T).Name);
                throw;
            }
        }

        public void Set(string key, string obj)
        {
            if (Preferences.ContainsKey(key))
            {
                _logger.LogDebug("Removing existing cache key before setting: {Key}", key);
                Preferences.Remove(key);
            }

            Preferences.Set(key, obj);
            _logger.LogDebug("Cache key set successfully: {Key}", key);
        }
    }
}
