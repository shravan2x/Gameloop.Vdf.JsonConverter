using Newtonsoft.Json.Linq;
using System;

namespace Gameloop.Vdf.JsonConverter
{
    public static class VTokenExtensions
    {
        public static JToken ToJson(this VToken tok)
        {
            switch (tok)
            {
                case VValue val:
                    return val.ToJson();
                case VProperty prop:
                    return prop.ToJson();
                case VObject obj:
                    return obj.ToJson();
                default:
                    throw new InvalidOperationException("Unrecognized VToken.");
            }
        }

        public static JValue ToJson(this VValue val)
        {
            return new JValue(val.Value);
        }

        public static JProperty ToJson(this VProperty prop)
        {
            return new JProperty(prop.Key, prop.Value.ToJson());
        }

        public static JObject ToJson(this VObject obj)
        {
            JObject resultObj = new JObject();

            foreach (VProperty prop in obj.Children())
                resultObj.Add(prop.ToJson());

            return resultObj;
        }
    }
}
