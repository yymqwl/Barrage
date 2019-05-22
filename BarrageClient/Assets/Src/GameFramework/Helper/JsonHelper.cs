using System;
using System.Text;
#if SERVER

namespace GameFramework
{
using Newtonsoft.Json;
    public static class JsonHelper
    {
        public static Byte[] JsonSerialize(object obj)
        {

            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));

        }
        public static T JsonDeserialize<T>(Byte[] data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data));
            }
            catch (Exception ex)
            {
                throw new GameFrameworkException("JsonDeserialize type: " + ex.ToString());
            }

        }
        public static string JsonSerialize_String(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static T JsonDeserialize_String<T>(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception ex)
            {
                throw new GameFrameworkException("JsonDeserialize type: " + ex.ToString());

            }
        }
    }
}
#else
namespace GameFramework
{
    using UnityEngine;
    public static class JsonHelper
    {
        public static Byte[] JsonSerialize(object obj)
        {
            
            return Encoding.UTF8.GetBytes(JsonSerialize_String(obj));

        }
        public static T JsonDeserialize<T>(Byte[] data)
        {
            try
            {
                return JsonDeserialize_String<T>(Encoding.UTF8.GetString(data));
            }
            catch (Exception ex)
            {
                throw new GameFrameworkException("JsonDeserialize type: " + ex.ToString());
            }

        }
        public static string JsonSerialize_String(object obj)
        {
            return JsonUtility.ToJson(obj);
        }
        public static T JsonDeserialize_String<T>(string data)
        {
            try
            {
                return JsonUtility.FromJson<T>(data);
            }
            catch (Exception ex)
            {
                throw new GameFrameworkException("JsonDeserialize type: " + ex.ToString());

            }
        }
    }
}
#endif