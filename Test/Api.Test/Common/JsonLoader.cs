using Newtonsoft.Json;

namespace Api.Test.Common;

public static class JsonLoader
{
    public static T LoadJson<T>(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<T>(json);
    }
}