using Newtonsoft.Json;

namespace D2.MasterData.Infrastructure
{
    public static class Json
    {
        static JsonSerializerSettings _settings;

        static Json()
        {
            _settings = new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        static public string Serialize(object instance)
        {
            return JsonConvert.SerializeObject(instance, _settings);
        }

        static public T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, _settings);
        }
    }
}
