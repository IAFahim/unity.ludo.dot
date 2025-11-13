/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

namespace  PlayFab
{
    public interface ISerializer
    {
        T DeserializeObject<T>(string json);
        T DeserializeObject<T>(string json, object jsonSerializerStrategy);

        string SerializeObject(object json);
        string SerializeObject(object json, object jsonSerializerStrategy);
    }

    public class SimpleJson
    {
        private static ISerializer _instance = new SimpleJsonInstance();

        /// <summary>
        /// Use this property to override the Serialization for the SDK.
        /// </summary>
        public static ISerializer Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        public static T DeserializeObject<T>(string json)
        {
            return _instance.DeserializeObject<T>(json);
        }

        public static T DeserializeObject<T>(string json, object jsonSerializerStrategy)
        {
            return _instance.DeserializeObject<T>(json, jsonSerializerStrategy);
        }

        public static string SerializeObject(object json)
        {
            return _instance.SerializeObject(json);
        }

        public static string SerializeObject(object json, object jsonSerializerStrategy)
        {
            return _instance.SerializeObject(json, jsonSerializerStrategy);
        }
    }

    public class SimpleJsonInstance : ISerializer
    {
        public T DeserializeObject<T>(string json)
        {
            return PlayFab.Json.SimpleJson.DeserializeObject<T>(json);
        }

        public T DeserializeObject<T>(string json, object jsonSerializerStrategy)
        {
            return PlayFab.Json.SimpleJson.DeserializeObject<T>(json, (PlayFab.Json.IJsonSerializerStrategy)jsonSerializerStrategy);
        }

        public string SerializeObject(object json)
        {
            return PlayFab.Json.SimpleJson.SerializeObject(json);
        }

        public string SerializeObject(object json, object jsonSerializerStrategy)
        {
            return PlayFab.Json.SimpleJson.SerializeObject(json, (PlayFab.Json.IJsonSerializerStrategy)jsonSerializerStrategy);
        }
    }


}
