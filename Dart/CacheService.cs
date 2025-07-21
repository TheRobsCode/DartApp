using System.Text.Json;
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
        public void Clear(string key)
        {
            Preferences.Remove(key);
        }

        public bool ContainsKey(string key)
        {
            return Preferences.ContainsKey(key);
        }

        public string Get(string key)
        {
            if (!Preferences.ContainsKey(key))
                return "";
            return Preferences.Get(key, "").ToString();
        }
        public T Get<T>(string key)
        {
            if (!Preferences.ContainsKey(key))
                return default;
            if (string.IsNullOrEmpty(Preferences.Get(key, "")))
                return default;

            var json = Preferences.Get(key, "").ToString();
            if (string.IsNullOrEmpty(json))
                return default;
            return JsonSerializer.Deserialize<T>(json);
        }
        public void Set<T>(string key, T obj)
        {
            var json = JsonSerializer.Serialize(obj);
            Set(key, json);
        }
        public void Set(string key, string obj)
        {
            if (Preferences.ContainsKey(key))
                Preferences.Remove(key);

            Preferences.Set(key, obj);
        }
    }
}
