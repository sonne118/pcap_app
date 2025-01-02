using Newtonsoft.Json;

namespace srv_sub;

internal sealed class Serializer : ISerializer
{
    public string Serialize<T>(T data)
        where T : class
    {
        return JsonConvert.SerializeObject(data);
    }

    public T Deserialize<T>(string data) where T : class
    {
        return JsonConvert.DeserializeObject<T>(data)!;
    }
}