namespace Sense.Storage {
    public interface IConfig {
        string Get(string key, string defaultValue = "");
        int GetInt(string key, int defaultValue = 0);
        void Set(string key, string value);
        void Set(string key, int value);
    }
}